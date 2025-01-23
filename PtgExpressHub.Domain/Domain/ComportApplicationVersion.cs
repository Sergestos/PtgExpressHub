namespace PtgExpressHub.Data.Domain;

public class ComportApplicationVersion
{
    public required Guid ApplicationVersionId { get; set; }

    public required Guid ApplicationId { get; set; }

    public required DateTime UploadDate { get; set; }

    public required string Version { get; set; }

    public required string BlobUrl { get; set; }
}
