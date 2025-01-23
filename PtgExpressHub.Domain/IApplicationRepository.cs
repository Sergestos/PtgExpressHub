using PtgExpressHub.Data.Domain;

namespace PtgExpressHub.Data;

public interface IApplicationRepository
{
    Task<IList<ComportApplication>> GetAllApplicationsAsync(CancellationToken cancellation);

    Task CreateApplicationAsync(ComportApplication application, CancellationToken cancellation);
}
