namespace PtgExpressHub.Domain.Entities;

public class ApplicationBuild
{
    public required Guid ApplicationBuildId { get; set; }

    public required string ApplicationBuildProductName { get; set; }

    public required string ApplicationBuildUserName { get; set; }

    public required string ApplicationRepositoryUrl { get; set; }

    public IList<ApplicationBuildVersion>? ApplicationBuildVersions { get; set; }
}
