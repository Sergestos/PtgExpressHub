namespace PtgExpressHub.Data.Domain;

public class ComportApplication
{
    public required Guid ApplicationId { get; set; }

    public required string ApplicationName { get; set; }

    public IList<ComportApplicationVersion>? ApplicationVersions { get; set; }
}
