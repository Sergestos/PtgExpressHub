using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PtgExpressHub.Data.Domain;

namespace PtgExpressHub.Data;

internal class ApplicationRepository : IApplicationRepository
{
    private CosmosDbContext _dbContext;

    public ApplicationRepository(CosmosDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task CreateApplicationAsync(ComportApplication application, CancellationToken cancellation)
    {
        return null;
    }

    public async Task<IList<ComportApplication>> GetAllApplicationsAsync(CancellationToken cancellation)
    {
        IOrderedQueryable<ComportApplication> queryable = _dbContext.GetContainer().GetItemLinqQueryable<ComportApplication>();
        using FeedIterator<ComportApplication> linqFeed = queryable.ToFeedIterator();

        var result = new List<ComportApplication>();
        while (linqFeed.HasMoreResults)
        {
            FeedResponse<ComportApplication> response = await linqFeed.ReadNextAsync();
            
            foreach (var item in response)
            {
                result.Add(item);
            }
        }

        return result;
    }
}
