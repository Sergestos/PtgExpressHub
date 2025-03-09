//using Microsoft.Azure.Cosmos;
//using Microsoft.Azure.Cosmos.Linq;
//using PtgExpressHub.Domain.Domain;

//namespace PtgExpressHub.Domain;

//public class ApplicationBuildRepository : IApplicationRepository
//{
//    private CosmosDbContext _dbContext;

//    public ApplicationBuildRepository(CosmosDbContext dbContext)
//    {
//        _dbContext = dbContext;
//    }

//    public async Task CreateApplicationBuildAsync(ApplicationBuild applicationBuild, CancellationToken cancellationToken)
//    {
//        PartitionKey partitionKey = new PartitionKey(applicationBuild.ApplicationBuildProductName);
//        ItemResponse<ApplicationBuild> response = await _dbContext.GetContainer().CreateItemAsync(applicationBuild, partitionKey, null, cancellationToken);
//    }

//    public async Task<IList<ApplicationBuild>> GetAllApplicationsBuildAsync(CancellationToken cancellation)
//    {
//        IOrderedQueryable<ApplicationBuild> queryable = _dbContext.GetContainer().GetItemLinqQueryable<ApplicationBuild>();
//        using FeedIterator<ApplicationBuild> linqFeed = queryable.ToFeedIterator();

//        var result = new List<ApplicationBuild>();
//        while (linqFeed.HasMoreResults)
//        {
//            FeedResponse<ApplicationBuild> response = await linqFeed.ReadNextAsync();
            
//            foreach (var item in response)
//            {
//                result.Add(item);
//            }
//        }

//        return result;
//    }
//}
