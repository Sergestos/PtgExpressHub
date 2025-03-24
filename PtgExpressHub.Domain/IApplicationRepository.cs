using PtgExpressHub.Domain.Entities;

namespace PtgExpressHub.Domain;

public interface IApplicationRepository
{
    Task<IList<ApplicationBuild>> GetAllApplicationsBuildAsync(CancellationToken cancellation);

    Task<ApplicationBuild?> GetApplicationsBuildByProductionNameAsync(string productionName, CancellationToken cancellation);

    Task<ApplicationBuild> CreateApplicationBuildAsync(ApplicationBuild applicationBuild, CancellationToken cancellation);

    Task<ApplicationBuildVersion> CreateApplicationBuildVersionAsync(Guid buildId, ApplicationBuildVersion applicationBuildVersion, CancellationToken cancellation);
}
