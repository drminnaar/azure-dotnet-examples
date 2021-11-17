using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using TableStorage.CrudApi.Services.GameReviews;

namespace TableStorage.CrudApi.Services.Games
{
    public sealed class GameReviewSummaryService
    {
        private readonly ILogger<GameReviewsService> _logger;
        private readonly TableClient _tableClient;
        private readonly IFakeSummaryGenerator _faker;

        public GameReviewSummaryService(
            ILogger<GameReviewsService> logger,
            TableClient tableClient,
            IFakeSummaryGenerator faker)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tableClient = tableClient ?? throw new ArgumentNullException(nameof(tableClient));
            _faker = faker ?? throw new ArgumentNullException(nameof(faker));
        }

        public async Task<GameReviewSummary> AddGameReviewSummary(GameReviewSummaryForCreate summary)
        {
            var entity = (GameReviewSummaryTableEntity)summary;
            await _tableClient.AddEntityAsync(entity.ToTableEntity());
            var response = await _tableClient.GetEntityAsync<TableEntity>(entity.PartitionKey, entity.RowKey);
            return new GameReviewSummary(response.Value);
        }

        public async Task AddGameReviewSummaryBatch(IReadOnlyCollection<GameReviewSummaryForCreate> summaries)
        {
            var batch = summaries
                .Select(summary => (GameReviewSummaryTableEntity)summary)
                .Select(summary => new TableTransactionAction(TableTransactionActionType.Add, summary.ToTableEntity()))
                .ToList();

            var response = await _tableClient.SubmitTransactionAsync(batch);
        }

        public async Task<GameReview?> DeleteGameReviewSummary(string platform, string title)
        {
            try
            {
                var summaryFromStorage = await _tableClient.GetEntityAsync<GameReviewTableEntity>(
                    partitionKey: platform.ToLowerInvariant(),
                    rowKey: title.ToLowerInvariant());

                await _tableClient.DeleteEntityAsync(platform, title, summaryFromStorage.Value.ETag);

                return (GameReview)summaryFromStorage.Value;
            }
            catch (RequestFailedException error) when (error.Status == (int)HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task GenerateData(int count)
        {
            var summaries = _faker.GenerateSummaries(count);

            var tasks = new List<Task>();

            foreach (var summary in summaries)
            {
                tasks.Add(
                    _tableClient
                        .AddEntityAsync(summary.ToTableEntity())
                        .ContinueWith(response =>
                        {
                            if (!response.IsCompletedSuccessfully)
                            {
                                var exception = response
                                    .Exception
                                    .Flatten()
                                    .InnerExceptions
                                    .FirstOrDefault();

                                _logger.LogError(exception, "Insert game review summary failed");
                            }
                        }));
            }
            await Task.WhenAll(tasks);
        }

        public async Task<GameReviewSummary?> GetGameReviewSummary(string platform, string title)
        {
            try
            {
                var summary = await _tableClient.GetEntityAsync<TableEntity>(
                    partitionKey: platform.ToLowerInvariant(),
                    rowKey: title.ToLowerInvariant());

                return summary.Value is null ? null : (GameReviewSummary)summary.Value;
            }
            catch (RequestFailedException error) when (error.Status == (int)HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IPagedCollection<GameReviewSummary>> GetGameReviewSummaries(GameReviewSummaryQuery query)
        {
            var filter = new GameReviewSummaryFilterBuilder()
                .WherePlatformEquals(query.Platform)
                .WhereTitleEquals(query.Title)
                .WhereMaxAverageUserRating(query.MaxAverageUserRating)
                .WhereMinAverageUserRating(query.MinAverageUserRating)
                .Filter;

            var count = await GetTotalSummaryCount(filter);

            var pages = _tableClient
                .QueryAsync<TableEntity>(filter)
                .AsPages(default, query.PageSize);

            var iteratorPageNumber = 1;
            await foreach (var page in pages)
            {
                if (query.PageNumber == iteratorPageNumber)
                {
                    var summaries = page
                        .Values
                        .Select(review => (GameReviewSummary)review)
                        .ToList();

                    return new PagedCollection<GameReviewSummary>(
                        summaries,
                        count,
                        query.PageNumber,
                        query.PageSize);
                }
                Interlocked.Increment(ref iteratorPageNumber);
            }

            return new EmptyPagedCollection<GameReviewSummary>();
        }

        private async Task<int> GetTotalSummaryCount(string filter)
        {
            var pages = _tableClient
                .QueryAsync<TableEntity>(filter)
                .AsPages();

            var count = 0;

            await foreach (var page in pages)
                Interlocked.Add(ref count, page.Values.Count);

            return count;
        }

        public async Task<GameReviewSummaryTableEntity?> UpdateGameReviewSummary(GameReviewSummaryForUpdate summary)
        {
            try
            {
                var response = await _tableClient.GetEntityAsync<TableEntity>(
                    partitionKey: summary.Platform.ToLowerInvariant(),
                    rowKey: summary.Title.ToLowerInvariant());

                var summaryFromStorage = new GameReviewSummaryTableEntity(response.Value);
                var update = summaryFromStorage.WithUpdate(summary);
                await _tableClient.UpdateEntityAsync(update.ToTableEntity(), update.ETag);
                return update;
            }
            catch (RequestFailedException error) when (error.Status == (int)HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    }
}
