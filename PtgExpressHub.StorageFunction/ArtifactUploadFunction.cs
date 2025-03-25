using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PtgExpressHub.Domain;
using PtgExpressHub.Domain.Entities;

namespace PtgExpressHub.StorageFunction;

public class ArtifactUploadFunction
{
    private readonly ILogger _logger;
    private readonly IApplicationRepository _applicationRepository;

    private readonly JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public ArtifactUploadFunction(ILoggerFactory loggerFactory, IApplicationRepository applicationRepository)
    {
        _logger = loggerFactory.CreateLogger<ArtifactUploadFunction>();
        _applicationRepository = applicationRepository;
    }

    [Function("upload-artifacts-function")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("HTTP trigger function processed a request.");

        HttpResponseData response = await HandleArtifactUploadAsync(request, cancellationToken);

        return response;
    }

    private async Task<HttpResponseData> HandleArtifactUploadAsync(HttpRequestData request, CancellationToken cancellationToken)
    {        
        var requestData = await JsonSerializer.DeserializeAsync<ArtifactUploadRequest>(request.Body, options);
        if (requestData == null)        
            return BuildResponse(request, HttpStatusCode.BadRequest, "Request data was not deserialized correctly.");

        string? lastVersion = null;
        var applicationBuild = await _applicationRepository.GetApplicationsBuildByProductionNameAsync(requestData.ApplicationBuildProductionName, cancellationToken);
        if (applicationBuild == null)
        {
            var build = new ApplicationBuild()
            {
                ApplicationBuildId = Guid.NewGuid(),
                ApplicationBuildProductionName = requestData.ApplicationBuildProductionName,
                ApplicationBuildUserName = requestData.ApplicationBuildUserName,
                ApplicationRepositoryUrl = requestData.RepositoryUrl,
            }; 

            applicationBuild = await _applicationRepository.CreateApplicationBuildAsync(build, cancellationToken);
        }
        else
        {
            lastVersion = applicationBuild.ApplicationBuildVersions!.OrderByDescending(x => x.UploadDate).First().Version;
        }

        var applicationBuildVersion = new ApplicationBuildVersion()
        {
            ApplicationVersionId = Guid.NewGuid(),
            BlobUrl = requestData.ApplicationBuildBlobPath,
            ChangeLog = requestData.ChangeLog,
            UploadDate = DateTime.UtcNow,
            ApplicationBuild = applicationBuild,
            Version = IncrementVersion(requestData.Version, lastVersion)
        };

        await _applicationRepository.CreateApplicationBuildVersionAsync(applicationBuild.ApplicationBuildId, applicationBuildVersion, cancellationToken);

        return BuildResponse(request, HttpStatusCode.OK, "Request has been completed successfully.");
    }

    private string IncrementVersion(string newVersion, string? lastVersion)
    {
        if (lastVersion == null)
            return newVersion;

        BuildVersion lastBuildVersion = BuildVersion.Parse(lastVersion);
        BuildVersion newBuildVersion = BuildVersion.Parse(newVersion);

        if (lastBuildVersion.Major == newBuildVersion.Major && lastBuildVersion.Minor == newBuildVersion.Minor)
            return lastBuildVersion.IncrementPatch().ToString();

        if (lastBuildVersion.Major == newBuildVersion.Major && lastBuildVersion.Minor != newBuildVersion.Minor)
            return lastBuildVersion.IncrementMinor().ToString();

        if (lastBuildVersion.Major != newBuildVersion.Major)
            return lastBuildVersion.IncrementMajor().ToString();

        return lastVersion;
    }

    private HttpResponseData BuildResponse(HttpRequestData request, HttpStatusCode httpStatusCode, string message)
    {
        var response = request.CreateResponse(httpStatusCode);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString(message);

        return response;
    }
}
