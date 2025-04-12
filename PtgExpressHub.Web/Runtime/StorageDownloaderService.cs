using Azure.Storage.Blobs;
using System.IO.Compression;

namespace PtgExpressHub.Web.Runtime;

public class StorageDownloaderService
{
    private readonly IConfiguration _configuration;

    public StorageDownloaderService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<byte[]> DownloadFromStorage(string fullStoragePath)
    {
        string appName = fullStoragePath.Split('/').Last();

        var blobServiceClient = new BlobServiceClient(_configuration["Storage:ConnectionString"]);
        var container = blobServiceClient.GetBlobContainerClient(_configuration["Storage:Folder"]);
        var blobs = container.GetBlobsByHierarchy()
            .Where(x => x.Blob.Name.StartsWith(appName))
            .Select(item => container.GetBlobClient(item.Blob.Name))
            .ToList();

        using var zipStream = new MemoryStream();
        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var blobItem in blobs)
            {
                var blobDownloadInfo = await blobItem.DownloadAsync();
                var zipEntry = archive.CreateEntry(blobItem.Name);

                using var entryStream = zipEntry.Open();
                await blobDownloadInfo.Value.Content.CopyToAsync(entryStream);
            }
        }

        return zipStream.ToArray();
    }
}
