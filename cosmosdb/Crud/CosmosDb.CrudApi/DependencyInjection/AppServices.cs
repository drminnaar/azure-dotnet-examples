using System;
using System.Threading.Tasks;
using Azure.Cosmos;
using CosmosDb.CrudApi.Services.Games;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CosmosDb.CrudApi.DependencyInjection;

public static class AppServices
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        var (client, databaseId, containerId) = InitializeCosmosDb(configuration).GetAwaiter().GetResult();

        return services
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddSingleton<ICosmosEntityIdProvider, CosmosEntityIdProvider>()
            .AddSingleton(client)
            .AddGamesService(databaseId, containerId);
    }

    private static async Task<(CosmosClient Client, string DatabaseId, string ContainerId)> InitializeCosmosDb(IConfiguration configuration)
    {
        var client = new CosmosClient(configuration.GetConnectionString("CosmosDb"));
        var database = await InitializeDatabase(client);
        var container = await InitializeGamesByPlatformContainer(client, database.Id);
        return (client, database.Id, container.Id);
    }

    private static async Task<DatabaseProperties> InitializeDatabase(CosmosClient cosmosClient)
    {
        const string DatabaseId = "gamesdb";
        const int DatabaseThroughput = 400;

        return await cosmosClient
            .CreateDatabaseIfNotExistsAsync(DatabaseId, DatabaseThroughput);
    }

    private static async Task<ContainerProperties> InitializeGamesByPlatformContainer(CosmosClient cosmosClient, string databaseId)
    {
        const string ContainerId = "gamesByPlatform";
        const string PartitionKeyPath = "/pk";

        return await cosmosClient
            .GetDatabase(databaseId)
            .CreateContainerIfNotExistsAsync(ContainerId, PartitionKeyPath);
    }

    private static IServiceCollection AddGamesService(this IServiceCollection services, string databaseId, string containerId)
    {
        services.AddSingleton<IGameService>(provider =>
            {
                var client = provider.GetRequiredService<CosmosClient>();
                var dateTimeProvider = provider.GetRequiredService<IDateTimeProvider>();
                var idProvider = provider.GetRequiredService<ICosmosEntityIdProvider>();
                var mapper = new GameMapper(dateTimeProvider, idProvider);
                var faker = new GameFaker();
                var containerWrapper = new CosmosContainerWrapper(client, databaseId, containerId);
                return new GameService(faker, mapper, containerWrapper);
            });

        return services;
    }
}
