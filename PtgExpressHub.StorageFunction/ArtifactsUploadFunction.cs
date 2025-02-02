using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace PtgExpressHub.StorageFunction;

public class ArtifactsUploadFunction
{
    [FunctionName("on-artifacts-uploaded-function")]
    public static void Run2([BlobTrigger("comport-application-blob/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob, string name, ILogger log)
    {
        log.LogInformation("C# BlobTrigger function processed a request.");

        log.LogInformation($"Blob Triggered: {name}");
        log.LogInformation($"Blob Size: {myBlob.Length} bytes");
    }
}
