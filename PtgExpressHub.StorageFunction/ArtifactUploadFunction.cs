using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PtgExpressHub.StorageFunction.Models;
using PtgExpressHub.StorageFunction.Services;

namespace PtgExpressHub.StorageFunction;

public class ArtifactUploadFunction
{
    private readonly ILogger _logger;
    private readonly IApplicationBuildService _applicationBuildService;

    private readonly JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public ArtifactUploadFunction(ILoggerFactory loggerFactory, IApplicationBuildService applicationBuildService)
    {
        _logger = loggerFactory.CreateLogger<ArtifactUploadFunction>();
        _applicationBuildService = applicationBuildService;
    }

    [Function("upload-artifacts-function")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("HTTP trigger function processed a request.");

        var requestData = await JsonSerializer.DeserializeAsync<ArtifactUploadRequest>(request.Body, options);
        if (requestData == null)
        {
            string message = "Error while deserialized request data";
            _logger.LogInformation(message);
            return BuildResponse(request, HttpStatusCode.BadRequest, message);
        }

        try
        {
            await _applicationBuildService.CreateAsync(requestData, cancellationToken);

            string message = "Request has been processed successfully.";
            _logger.LogInformation(message);
            return BuildResponse(request, HttpStatusCode.OK, message);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return BuildResponse(request, HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    private HttpResponseData BuildResponse(HttpRequestData request, HttpStatusCode httpStatusCode, string message)
    {
        var response = request.CreateResponse(httpStatusCode);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString(message);

        return response;
    }
}
