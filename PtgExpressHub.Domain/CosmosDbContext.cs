using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;

public class CosmosDbContext
{
    private const string DataBaseName = "ApplicationsContainer";
    private const string ContainerName = "ApplicationsContainer";

    private CosmosClient? _client;
    private Container? _container;

    private string _accountEndpoint = string.Empty;

    private CosmosClient Client => _client ??= new CosmosClientBuilder(_accountEndpoint).Build();    

    public CosmosDbContext(IConfiguration configuration)
    {
        if (configuration != null)
            _accountEndpoint = configuration["CosmosDb:ConnectionString"]!;
    }

    public void SetAccountEndpoint(string accountEndpoint)
    {
        _accountEndpoint = accountEndpoint;
    }

    public Container GetContainer()
    {
        var dateBase = Client.GetDatabase(DataBaseName);
        return dateBase.GetContainer(ContainerName);
    }
}
