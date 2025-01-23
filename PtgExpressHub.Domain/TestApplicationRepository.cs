using PtgExpressHub.Data.Domain;

namespace PtgExpressHub.Data;

public class TestApplicationRepository : IApplicationRepository
{
    public Task CreateApplicationAsync(ComportApplication application, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    public Task<IList<ComportApplication>> GetAllApplicationsAsync(CancellationToken cancellation)
    {
        IList<ComportApplication> applications = [
            GenerateApplication("Comport213"), 
            GenerateApplication("Comport923"),
            GenerateApplication("Comport113"),
            GenerateApplication("Comport1001", DateTime.Now.AddDays(-5)),
            GenerateApplication("Comport99"),
            GenerateApplication("Comport422", DateTime.Now.AddDays(5)),
            GenerateApplication("Comport604"),
            GenerateApplication("Comport500")];

        return Task.FromResult(applications);
    }

    private ComportApplication GenerateApplication(string name, DateTime? dateTime = null)
    {
        var guid = Guid.NewGuid();
        var application = new ComportApplication()
        {
            ApplicationId = guid,
            ApplicationName = name,
            ApplicationVersions = new List<ComportApplicationVersion>()
            {
                new ComportApplicationVersion()
                {
                    ApplicationId = guid,
                    ApplicationVersionId = Guid.NewGuid(),
                    BlobUrl = "url://blob",
                    Version = "1.0.0",
                    UploadDate = dateTime != null ? dateTime.Value.AddDays(-1) : DateTime.Now.AddDays(-1),
                },
                new ComportApplicationVersion()
                {
                    ApplicationId = guid,
                    ApplicationVersionId = Guid.NewGuid(),
                    BlobUrl = "url://blob",
                    Version = "1.0.1",
                    UploadDate = dateTime ?? DateTime.Now,
                }
            }
        };

        return application;
    }
}
