using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagerFncAppV5.Data.Models;

namespace ProductManagerFncAppV5.Data;

internal static class CosmosDbServiceSetup
{
    public static IServiceCollection AddCosmosDb(this IServiceCollection services, IConfiguration configuration)
    {
        var (client, databaseId, containerId) = InitializeCosmosDb(configuration).GetAwaiter().GetResult();
        services.AddSingleton<ICosmosDbService<ProductEntity>>(new CosmosDbService<ProductEntity>(client, databaseId, containerId));
        return services;
    }

    private static async Task<(CosmosClient Client, string DatabaseId, string ContainerId)> InitializeCosmosDb(IConfiguration configuration)
    {
        var client = new CosmosClient(configuration.GetConnectionString("inventory"));
        var database = await InitializeDatabase(client);
        var container = await InitializeContainer(client, database.Id);
        return (client, database.Id, container.Id);
    }

    private static async Task<DatabaseProperties> InitializeDatabase(CosmosClient cosmosClient)
    {
        const string DatabaseId = "inventory";
        const int DatabaseThroughput = 400;

        return await cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId, DatabaseThroughput);
    }

    private static async Task<ContainerProperties> InitializeContainer(CosmosClient cosmosClient, string databaseId)
    {
        const string ContainerId = "products";
        const string PartitionKeyPath = "/pk";

        return await cosmosClient
            .GetDatabase(databaseId)
            .CreateContainerIfNotExistsAsync(ContainerId, PartitionKeyPath);
    }
}
