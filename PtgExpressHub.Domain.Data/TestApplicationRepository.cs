using PtgExpressHub.Domain;
using PtgExpressHub.Domain.Entities;

namespace PtgExpressHub.Domain;

public class TestApplicationRepository : IApplicationRepository
{
    public Task CreateApplicationBuildAsync(ApplicationBuild application, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    public Task<ApplicationBuildVersion> CreateApplicationBuildVersionAsync(Guid buildId, ApplicationBuildVersion applicationBuildVersion, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    public Task<IList<ApplicationBuild>> GetAllApplicationsBuildAsync(CancellationToken cancellation)
    {
        IList<ApplicationBuild> applications = [
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

    public Task<ApplicationBuild> GetApplicationsBuildByProductionNameAsync(string productName, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    Task<ApplicationBuild> IApplicationRepository.CreateApplicationBuildAsync(ApplicationBuild applicationBuild, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    private ApplicationBuild GenerateApplication(string name, DateTime? dateTime = null)
    {
        var guid = Guid.NewGuid();
        var application = new ApplicationBuild()
        {
            ApplicationBuildId = guid,
            ApplicationRepositoryUrl = "url://github",
            ApplicationBuildProductionName = name,
            ApplicationBuildUserName = "Product" + name.Skip(7),
            ApplicationBuildVersions = new List<ApplicationBuildVersion>()
            {
                new ApplicationBuildVersion()
                {
                    ApplicationBuild = null,
                    ApplicationBuildId = guid,
                    ApplicationVersionId = Guid.NewGuid(),
                    BlobUrl = "url://blob",
                    Version = "1.0.0",
                    UploadDate = dateTime != null ? dateTime.Value.AddDays(-1) : DateTime.Now.AddDays(-1),
                },
                new ApplicationBuildVersion()
                {
                    ApplicationBuild = null,
                    ApplicationBuildId = guid,
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
