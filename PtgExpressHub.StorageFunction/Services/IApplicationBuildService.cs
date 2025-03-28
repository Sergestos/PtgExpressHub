using PtgExpressHub.StorageFunction.Models;

namespace PtgExpressHub.StorageFunction.Services;

public interface IApplicationBuildService
{
    Task CreateAsync(ArtifactUploadRequest requestData, CancellationToken cancellationToken);
}
