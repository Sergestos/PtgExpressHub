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

        var applicationBuild = await _applicationRepository.GetApplicationsBuildByProductionNameAsync(requestData.ApplicationBuildName, cancellationToken);
        if (applicationBuild == null)
        {

        }

        ApplicationBuild build = new ApplicationBuild()
        {
            ApplicationBuildId = Guid.NewGuid(),
            ApplicationBuildProductionName = requestData.ApplicationBuildName,
            ApplicationBuildUserName = requestData.ApplicationBuildName,
            ApplicationRepositoryUrl = requestData.RepositoryUrl,
        };

        await _applicationRepository.CreateApplicationBuildAsync(null, cancellationToken);

        return BuildResponse(request, HttpStatusCode.BadRequest, "Request has been completed successfully.");
    }

    private HttpResponseData BuildResponse(HttpRequestData request, HttpStatusCode httpStatusCode, string message)
    {
        var response = request.CreateResponse(httpStatusCode);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString(message);

        return response;
    }
}
