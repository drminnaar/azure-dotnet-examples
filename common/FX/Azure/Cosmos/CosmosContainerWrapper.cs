using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Cosmos;

public interface ICosmosContainerWrapper
{
    string ContainerId { get; }
    string DatabaseId { get; }

    Task<ItemResponse<T>> CreateItemAsync<T>(T item, PartitionKey? partitionKey = null, ItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default);
    Task<ItemResponse<T>> DeleteItemAsync<T>(string id, PartitionKey partitionKey, ItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default);
    AsyncPageable<T> GetItemQueryIterator<T>(QueryDefinition queryDefinition, string? continuationToken = null, QueryRequestOptions? requestOptions = null, CancellationToken cancellationToken = default) where T : notnull;
    Task<int> GetTotalCountAsync<T>(QueryDefinition queryDefinition);
    Task<ItemResponse<T>> ReadItemAsync<T>(string id, PartitionKey partitionKey, ItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default);
    Task<IPagedCollection<T>> ReadItemsAsync<T>(CosmosQueryInput input);
    Task<ItemResponse<T>> UpsertItemAsync<T>(T item, PartitionKey? partitionKey = null, ItemRequestOptions? requestOptions = null, CancellationToken cancellationToken = default);
}

public sealed class CosmosContainerWrapper : ICosmosContainerWrapper
{
    private readonly CosmosClient _cosmosClient;
    private readonly CosmosContainer _container;
    private readonly string _databaseId;
    private readonly string _containerId;

    public CosmosContainerWrapper(CosmosClient cosmosClient, string databaseId, string containerId)
    {
        if (string.IsNullOrWhiteSpace(databaseId))
        {
            throw new ArgumentException($"'{nameof(databaseId)}' cannot be null or whitespace.", nameof(databaseId));
        }

        if (string.IsNullOrWhiteSpace(containerId))
        {
            throw new ArgumentException($"'{nameof(containerId)}' cannot be null or whitespace.", nameof(containerId));
        }

        _cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
        _databaseId = databaseId;
        _containerId = containerId;
        _container = _cosmosClient.GetContainer(databaseId, containerId);
    }

    public string DatabaseId => _databaseId;
    public string ContainerId => _containerId;

    public Task<ItemResponse<T>> CreateItemAsync<T>(
        T item,
        PartitionKey? partitionKey = null,
        ItemRequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
            _container.CreateItemAsync(item, partitionKey, requestOptions, cancellationToken);

    public Task<ItemResponse<T>> DeleteItemAsync<T>(
        string id,
        PartitionKey partitionKey,
        ItemRequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
            _container.DeleteItemAsync<T>(id, partitionKey, requestOptions, cancellationToken);

    public Task<ItemResponse<T>> ReadItemAsync<T>(
        string id,
        PartitionKey partitionKey,
        ItemRequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
            _container.ReadItemAsync<T>(id, partitionKey, requestOptions, cancellationToken);

    public AsyncPageable<T> GetItemQueryIterator<T>(
        QueryDefinition queryDefinition,
        string? continuationToken = null,
        QueryRequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) where T : notnull =>
            _container.GetItemQueryIterator<T>(queryDefinition, continuationToken, requestOptions, cancellationToken);

    public async Task<int> GetTotalCountAsync<T>(QueryDefinition queryDefinition)
    {
        var pages = _container
            .GetItemQueryIterator<T>(queryDefinition)
            .AsPages();

        var count = 0;

        await foreach (var page in pages)
        {
            Interlocked.Add(ref count, page.Values.Count);
        }

        return count;
    }

    public async Task<IPagedCollection<T>> ReadItemsAsync<T>(CosmosQueryInput input)
    {
        const string Query = "select * from c where c.pk = @pk";

        var queryDefinition = new QueryDefinition(Query)
            .WithParameter("@pk", input.PartitionKey);

        var totalItemCount = await GetTotalCountAsync<T>(queryDefinition);

        var pages = _container
            .GetItemQueryIterator<T>(
                queryDefinition,
                requestOptions: new QueryRequestOptions { MaxItemCount = input.PageSize })
            .AsPages(default, input.PageSize);

        var iteratorPageNumber = 1;

        await foreach (var page in pages)
        {
            if (input.PageNumber == iteratorPageNumber)
            {
                var pageItems = page.Values.ToList();

                return new PagedCollection<T>(
                    items: pageItems,
                    itemCount: totalItemCount,
                    pageNumber: input.PageNumber, pageSize: pageItems.Count);
            }
            Interlocked.Increment(ref iteratorPageNumber);
        }

        return new EmptyPagedCollection<T>();
    }

    public Task<ItemResponse<T>> UpsertItemAsync<T>(
        T item,
        PartitionKey? partitionKey = null,
        ItemRequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default) =>
            _container.UpsertItemAsync<T>(item, partitionKey, requestOptions, cancellationToken);
}
