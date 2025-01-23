using PtgExpressHub.Data.Domain;

namespace PtgExpressHub.Data;

public class ApplicationRepository
{
    public Task<IList<Application>> GetAllApplicationsAsync(CancellationToken cancellation)
    {
        IList<Application> applications = [
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

    private Application GenerateApplication(string name, DateTime? dateTime = null)
    {
        var guid = Guid.NewGuid();
        var application = new Application()
        {
            ApplicationId = guid,
            ApplicationName = name,
            ApplicationVersions = new List<ApplicationVersion>()
            {
                new ApplicationVersion()
                {
                    ApplicationId = guid,
                    ApplicationVersionId = Guid.NewGuid(),
                    BlobUrl = "url://blob",
                    Version = "1.0.0",
                    UploadDate = dateTime != null ? dateTime.Value.AddDays(-1) : DateTime.Now.AddDays(-1),
                },
                new ApplicationVersion()
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
