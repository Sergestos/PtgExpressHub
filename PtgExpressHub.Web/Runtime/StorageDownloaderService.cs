using Azure.Storage.Blobs;

namespace PtgExpressHub.Web.Runtime;

public class StorageDownloaderService
{
    private readonly IConfiguration _configuration;

    public StorageDownloaderService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Stream?> DownloadFromStorage(string storagePath)
    {
        var blobServiceClient = new BlobServiceClient(_configuration["Storage:ConnectionString"]);
        var container = blobServiceClient.GetBlobContainerClient(_configuration["Storage:Folder"]);
        var blobs = container.GetBlobsByHierarchy(prefix: storagePath.Split('/').Last(), delimiter: "/").ToList()
                           .Where(item => item.IsBlob) 
                           .Select(item => container.GetBlobClient(item.Blob.Name))
                           .ToList();

        var combinedStream = new MemoryStream();
        foreach (var blob in blobs)
        {
            var blobDownloadInfo = await blob.DownloadAsync();
            await blobDownloadInfo.Value.Content.CopyToAsync(combinedStream);
        }

        combinedStream.Position = 0;
        return combinedStream;
    }
}
