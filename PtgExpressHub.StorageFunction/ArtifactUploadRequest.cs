namespace PtgExpressHub.StorageFunction;

internal class ArtifactUploadRequest
{
    public required string ApplicationBuildName { get; set; }

    public required string ApplicationBuildPath { get; set; }

    public string? Comment { get; set; }

    public required string Version { get; set; }

    public required string RepositoryUrl { get; set; }    
}
