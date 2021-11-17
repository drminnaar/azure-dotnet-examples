
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;

namespace TableStorage.CrudApi.Services.GameReviews;

public sealed class GameReviewsService
{
    private readonly ILogger<GameReviewsService> _logger;
    private readonly TableClient _tableClient;
    private readonly IGameReviewTableEntityFactory _factory;
    private readonly IFakeReviewGenerator _faker;

    public GameReviewsService(
        ILogger<GameReviewsService> logger,
        TableClient tableClient,
        IGameReviewTableEntityFactory factory,
        IFakeReviewGenerator faker)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _tableClient = tableClient ?? throw new ArgumentNullException(nameof(tableClient));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _faker = faker ?? throw new ArgumentNullException(nameof(faker));
    }

    public async Task<GameReview> AddGameReview(GameReviewForCreate review)
    {
        var reviewForCreate = _factory.Create(review);
        await _tableClient.AddEntityAsync(reviewForCreate);
        var reviewFromStorage = await _tableClient.GetEntityAsync<GameReviewTableEntity>(
            reviewForCreate.PartitionKey,
            reviewForCreate.RowKey);
        return (GameReview)reviewFromStorage.Value;
    }

    public async Task AddGameReviewBatch(IReadOnlyCollection<GameReviewForCreate> reviews)
    {
        var batch = reviews
            .Select(review => _factory.Create(review))
            .Select(review => new TableTransactionAction(TableTransactionActionType.Add, review))
            .ToList();

        var response = await _tableClient.SubmitTransactionAsync(batch);
    }

    public async Task<GameReviewTableEntity?> UpdateGameReview(GameReviewForUpdate review)
    {
        try
        {
            var reviewFromStorage = await _tableClient.GetEntityAsync<GameReviewTableEntity>(
                partitionKey: review.Platform.ToLowerInvariant(),
                rowKey: review.ReviewId);

            var update = reviewFromStorage.Value.WithUpdate(review);
            await _tableClient.UpdateEntityAsync(update, update.ETag);
            return update;
        }
        catch (RequestFailedException error) when (error.Status == (int)HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task GenerateData(int count)
    {
        var reviews = _faker.GenerateReviews(count);

        var tasks = new List<Task>();

        foreach (var review in reviews)
        {
            tasks.Add(_tableClient
                .AddEntityAsync(review)
                .ContinueWith(response =>
                {
                    if (!response.IsCompletedSuccessfully)
                    {
                        var exception = response
                            .Exception
                            .Flatten()
                            .InnerExceptions
                            .FirstOrDefault();

                        _logger.LogError(exception, "Insert game review failed");
                    }
                }));
        }
        await Task.WhenAll(tasks);
    }

    public async Task<GameReview?> DeleteGameReview(string platform, string reviewId)
    {
        try
        {
            var reviewFromStorage = await _tableClient.GetEntityAsync<GameReviewTableEntity>(
                partitionKey: platform,
                rowKey: reviewId);

            await _tableClient.DeleteEntityAsync(platform, reviewId, reviewFromStorage.Value.ETag);

            return (GameReview)reviewFromStorage.Value;
        }
        catch (RequestFailedException error) when (error.Status == (int)HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<GameReview?> GetGameReview(string partitionKey, string rowKey)
    {
        try
        {
            var review = await _tableClient.GetEntityAsync<GameReviewTableEntity>(
                partitionKey.ToLowerInvariant(),
                rowKey.ToLowerInvariant());

            return review.Value is null ? null : (GameReview)review.Value;
        }
        catch (RequestFailedException error) when (error.Status == (int)HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<IPagedCollection<GameReview>> GetGameReviews(GameReviewQuery query)
    {
        var filter = new GameReviewQueryBuilder()
            .WherePlatformEquals(query.Platform)
            .WhereMaxReviewedAtEquals(query.To)
            .WhereMinReviewedAtEquals(query.From)
            .WhereUserIdEquals(query.UserId)
            .WhereTitleEquals(query.Title)
            .Filter;

        var reviewCount = await GetTotalReviewCount(filter);

        var reviewPages = _tableClient.QueryAsync(filter).AsPages(default, query.PageSize);

        var iteratorPageNumber = 1;
        await foreach (var reviewPage in reviewPages)
        {
            if (query.PageNumber == iteratorPageNumber)
            {
                var reviews = reviewPage
                    .Values
                    .Select(review => (GameReview)review)
                    .ToList();

                return new PagedCollection<GameReview>(reviews, reviewCount, query.PageNumber, query.PageSize);
            }
            Interlocked.Increment(ref iteratorPageNumber);
        }

        return new EmptyPagedCollection<GameReview>();
    }

    private async Task<int> GetTotalReviewCount(Expression<Func<GameReviewTableEntity, bool>> filter)
    {
        var reviewPages = _tableClient
            .QueryAsync(filter)
            .AsPages();

        var count = 0;

        await foreach (var reviewPage in reviewPages)
        {
            Interlocked.Add(ref count, reviewPage.Values.Count);
        }

        return count;
    }
}
