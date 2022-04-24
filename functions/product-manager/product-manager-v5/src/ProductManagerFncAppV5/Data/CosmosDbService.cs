using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Newtonsoft.Json;

namespace ProductManagerFncAppV5.Data;

public interface ICosmosDbItem
{
    [JsonProperty("id")]
    string Id { get; set; }

    [JsonProperty("pk")]
    string Pk { get; set; }
}

internal interface ICosmosDbService<T> where T : ICosmosDbItem, new()
{
    Task BulkInsertAsync(IReadOnlyCollection<T> items);
    Task<int> GetItemCountAsync(string partitionKey = "");
    Task<T> GetItemAsync(string id, PartitionKey partitionKey);
    Task<IReadOnlyCollection<T>> GetItemsAsync(QueryDefinition queryDefinition);
    Task<IPagedCollection<T>> GetItemsAsync(int pageNumber, int pageSize, string partitionKey = "");
    Task CreateItemAsync(T item, PartitionKey partitionKey);
    Task PurgeAsync();
    Task UpdateItemAsync(T item, PartitionKey partitionKey);
    Task DeleteItemAsync(string id, PartitionKey partitionKey);
}

internal sealed class CosmosDbService<T> : ICosmosDbService<T> where T : ICosmosDbItem, new()
{
    private readonly Container _container;

    public CosmosDbService(
        CosmosClient dbClient,
        string databaseName,
        string containerName)
    {
        if (dbClient is null)
            throw new ArgumentNullException(nameof(dbClient));

        if (string.IsNullOrEmpty(databaseName))
            throw new ArgumentException($"'{nameof(databaseName)}' cannot be null or empty.", nameof(databaseName));

        if (string.IsNullOrEmpty(containerName))
            throw new ArgumentException($"'{nameof(containerName)}' cannot be null or empty.", nameof(containerName));

        _container = dbClient.GetContainer(databaseName, containerName);
    }

    public async Task DeleteItemAsync(string id, PartitionKey partitionKey)
    {
        await _container.DeleteItemAsync<T>(id, partitionKey);
    }

    public async Task<int> GetItemCountAsync(string partitionKey = "")
    {
        var queryDefinition = string.IsNullOrEmpty(partitionKey)
            ? new QueryDefinition("SELECT VALUE COUNT(1) FROM c")
            : new QueryDefinition("SELECT VALUE COUNT(1) FROM c WHERE c.pk = @pk").WithParameter("@pk", partitionKey);

        var query = _container.GetItemQueryIterator<int>(queryDefinition);

        while (query.HasMoreResults)
        {
            var queryResponse = await query.ReadNextAsync();
            return queryResponse.Resource.FirstOrDefault();
        }

        return default;
    }

    public async Task<T> GetItemAsync(string id, PartitionKey partitionKey)
    {
        try
        {
            var response = await _container.ReadItemAsync<T>(id, partitionKey);
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }
    }

    public async Task<IReadOnlyCollection<T>> GetItemsAsync(QueryDefinition queryDefinition)
    {
        var query = _container.GetItemQueryIterator<T>(queryDefinition);
        var results = new List<T>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.Resource);
        }
        return results;
    }

    public async Task<IPagedCollection<T>> GetItemsAsync(
        int pageNumber,
        int pageSize,
        string partitionKey = "")
    {
        var itemCount = await GetItemCountAsync(partitionKey);

        var query = string.IsNullOrWhiteSpace(partitionKey)
            ? _container
                .GetItemLinqQueryable<T>()
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToFeedIterator()
            : _container
                .GetItemLinqQueryable<T>()
                .Where(item => item.Pk == partitionKey)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToFeedIterator();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            return new PagedCollection<T>(response.Resource.ToList(), itemCount, pageNumber, pageSize);
        }
        return PagedCollection<T>.Empty;
    }

    public async Task CreateItemAsync(T item, PartitionKey partitionKey)
    {
        await _container.CreateItemAsync(item, partitionKey);
    }

    public async Task UpdateItemAsync(T item, PartitionKey partitionKey)
    {
        await _container.UpsertItemAsync<T>(item, partitionKey);
    }

    public async Task BulkInsertAsync(IReadOnlyCollection<T> items)
    {
        await ProcessBatch(
            items,
            (item) => _container.CreateItemAsync<T>(item, new PartitionKey(item.Pk)));
    }

    public async Task PurgeAsync()
    {
        var items = new List<T>();

        var query = new QueryDefinition("SELECT * FROM c");
        var iter = _container.GetItemQueryIterator<T>(query);

        while (iter.HasMoreResults)
        {
            var item = await iter.ReadNextAsync();
            items.AddRange(item.Resource);
        }

        await ProcessBatch(
            items,
            (item) => _container.DeleteItemAsync<T>(item.Id, new PartitionKey(item.Pk)));
    }

    private static async Task ProcessBatch(IReadOnlyCollection<T> items, Func<T, Task> createTask)
    {
        var batchSize = 50;
        var batchCount = (int)Math.Ceiling((double)items.Count / batchSize);
        var currentBatch = 0;
        while (currentBatch != batchCount)
        {
            var batch = items
                .Skip(currentBatch * batchSize)
                .Take(batchSize)
                .ToList();

            var tasks = new List<Task>();

            foreach (var item in batch)
            {
                tasks.Add(createTask(item).ContinueWith(response =>
                {
                    if (!response.IsCompletedSuccessfully)
                    {
                        var exception = response
                            .Exception
                            ?.Flatten()
                            .InnerExceptions
                            .FirstOrDefault();

                        Debug.WriteLine(exception);
                    }
                }));
            }

            await Task.WhenAll(tasks);

            Interlocked.Increment(ref currentBatch);
        }
    }
}
