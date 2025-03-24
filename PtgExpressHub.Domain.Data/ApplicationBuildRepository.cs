using Microsoft.EntityFrameworkCore;
using PtgExpressHub.Domain.Entities;

namespace PtgExpressHub.Domain;

public class ApplicationBuildRepository : IApplicationRepository
{
    private PtgExpressDataContext _dbContext;

    public ApplicationBuildRepository(PtgExpressDataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApplicationBuild> CreateApplicationBuildAsync(ApplicationBuild applicationBuild, CancellationToken cancellationToken)
    {
        var result = _dbContext.ApplicationBuilds.Add(applicationBuild);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return result.Entity;
    }

    public async Task<ApplicationBuildVersion> CreateApplicationBuildVersionAsync(Guid buildId, ApplicationBuildVersion applicationBuildVersion, CancellationToken cancellationToken)
    {
        var applicationBuild = _dbContext.ApplicationBuilds.FirstOrDefault(x => x.ApplicationBuildId == buildId);
        if (applicationBuild == null)
            throw new InvalidOperationException("ApplicationBuild with provided Id is not found");

        _dbContext.ApplicationBuildVersions.Add(applicationBuildVersion);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return applicationBuildVersion;
    }

    public async Task<IList<ApplicationBuild>> GetAllApplicationsBuildAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.ApplicationBuilds
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<ApplicationBuild?> GetApplicationsBuildByProductionNameAsync(string productionName, CancellationToken cancellationToken)
    {
        return await _dbContext.ApplicationBuilds!
            .FirstOrDefaultAsync(x => x.ApplicationBuildProductionName == productionName, cancellationToken);
    }
}
