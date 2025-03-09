using PtgExpressHub.Domain.Entities;

namespace PtgExpressHub.Domain;

public interface IApplicationRepository
{
    Task<IList<ApplicationBuild>> GetAllApplicationsBuildAsync(CancellationToken cancellation);

    Task<ApplicationBuild> GetApplicationsBuildByProductNameAsync(string productName, CancellationToken cancellation);

    Task CreateApplicationBuildAsync(ApplicationBuild applicationBuild, CancellationToken cancellation);
}
