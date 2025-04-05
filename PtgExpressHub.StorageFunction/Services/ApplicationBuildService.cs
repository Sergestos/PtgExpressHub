using PtgExpressHub.Domain;
using PtgExpressHub.Domain.Entities;
using PtgExpressHub.StorageFunction.Models;

namespace PtgExpressHub.StorageFunction.Services;

public sealed class ApplicationBuildService : IApplicationBuildService
{
    private readonly IApplicationRepository _applicationRepository;

    public ApplicationBuildService(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task CreateAsync(ArtifactUploadRequest requestData, CancellationToken cancellationToken)
    {
        string? lastVersion = null;
        var applicationBuild = await _applicationRepository.GetApplicationsBuildByProductionNameAsync(requestData.ApplicationBuildProductionName, cancellationToken);
        if (applicationBuild == null)
        {
            var build = new ApplicationBuild()
            {
                ApplicationBuildId = Guid.NewGuid(),
                ApplicationBuildProductionName = requestData.ApplicationBuildProductionName,
                ApplicationBuildUserName = requestData.ApplicationBuildUserName,
                ApplicationRepositoryUrl = requestData.RepositoryUrl,
            };

            applicationBuild = await _applicationRepository.CreateApplicationBuildAsync(build, cancellationToken);
        }
        else
        {
            lastVersion = applicationBuild.ApplicationBuildVersions!.OrderByDescending(x => x.UploadDate).First().Version;
        }

        var formattedVersion = string.Join(".", requestData.Version.Split('.').Take(3));
        var applicationBuildVersion = new ApplicationBuildVersion()
        {
            ApplicationVersionId = Guid.NewGuid(),
            BlobUrl = requestData.ApplicationBuildBlobPath,
            ChangeLog = requestData.ChangeLog,
            UploadDate = DateTime.UtcNow,
            ApplicationBuild = applicationBuild,
            Version = IncrementVersion(formattedVersion, lastVersion)
        };

        await _applicationRepository.CreateApplicationBuildVersionAsync(applicationBuild.ApplicationBuildId, applicationBuildVersion, cancellationToken);
    }

    private string IncrementVersion(string newVersion, string? lastVersion)
    {
        if (lastVersion == null)
            return newVersion;

        BuildVersion lastBuildVersion = BuildVersion.Parse(lastVersion);
        BuildVersion newBuildVersion = BuildVersion.Parse(newVersion);

        if (lastBuildVersion.Major != newBuildVersion.Major)
            return lastBuildVersion.IncrementMajor().ToString();

        if (lastBuildVersion.Major == newBuildVersion.Major && lastBuildVersion.Minor != newBuildVersion.Minor)
            return lastBuildVersion.IncrementMinor().ToString();

        if (lastBuildVersion.Major == newBuildVersion.Major && lastBuildVersion.Minor == newBuildVersion.Minor)
            return lastBuildVersion.IncrementPatch().ToString();        

        return lastVersion;
    }
}
