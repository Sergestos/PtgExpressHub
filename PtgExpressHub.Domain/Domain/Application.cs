namespace PtgExpressHub.Data.Domain;

public class Application
{
    public required Guid ApplicationId { get; set; }

    public required string ApplicationName { get; set; }

    public IList<ApplicationVersion>? ApplicationVersions { get; set; }
}
