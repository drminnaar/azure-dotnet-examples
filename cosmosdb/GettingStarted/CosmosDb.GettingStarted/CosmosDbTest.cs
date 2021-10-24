using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Cosmos;
using FakeData;
using FakeData.Reviews;
using Spectre.Console;

namespace CosmosDb.GettingStarted
{
    public sealed class CosmosDbTest
    {
        private readonly CosmosClient _cosmosClient;
        private readonly FakeEntityGeneratorBase<ReviewForCosmos> _faker;

        public CosmosDbTest(CosmosClient cosmosClient, FakeEntityGeneratorBase<ReviewForCosmos> faker)
        {
            _cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
            _faker = faker ?? throw new ArgumentNullException(nameof(faker));
        }

        public async Task Run()
        {
            const string DatabaseId = "reviewsdb";
            const int DatabaseThroughput = 400;
            const string ContainerId = "reviewsByType";

            // Create Database
            await CreateDatabaseIfNotExists(DatabaseId, DatabaseThroughput);

            // Create Container
            await CreateContainerIfNotExists(DatabaseId, ContainerId);

            // Create Reviews
            var reviews = await CreateReviews(DatabaseId, ContainerId);

            // Get All Reviews
            await GetReviews(DatabaseId, ContainerId);

            // Get All Game Reviews Rated higher than 7
            await GetReviewsHavingRatingGreaterThan(DatabaseId, ContainerId, "Game", 7);

            // Get All Book Reviews Rated higher than 8
            await GetReviewsHavingRatingGreaterThan(DatabaseId, ContainerId, "Book", 5);

            // Delete Reviews
            await DeleteReviews(DatabaseId, ContainerId, reviews);

            // Delete Container
            await DeleteContainer(DatabaseId, ContainerId);

            // Delete Database
            await DeleteDatabase(DatabaseId);
        }

        private async Task<CosmosDatabase> CreateDatabaseIfNotExists(string databaseId, int throughput)
        {
            ShowProcessStarted("CREATE DATABASE");

            var database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId, throughput);

            ShowProcessCompleted("CREATE DATABASE");

            return database;
        }

        private async Task<CosmosContainer> CreateContainerIfNotExists(string databaseId, string containerId)
        {
            ShowProcessStarted("CREATE CONTAINER");

            const string PartitionKeyPath = "/pk";

            var container = await _cosmosClient
                .GetDatabase(databaseId)
                .CreateContainerIfNotExistsAsync(containerId, PartitionKeyPath);

            ShowProcessCompleted("CREATE CONTAINER");

            return container;
        }

        private async Task<IReadOnlyCollection<ReviewForCosmos>> CreateReviews(string databaseId, string containerId)
        {
            ShowProcessStarted("CREATE ITEMS");

            var reviews = _faker.GenerateFakes(100);

            var container = _cosmosClient.GetContainer(databaseId, containerId);

            foreach (var review in reviews)
            {
                review.Pk = review.Type;
                var response = await container.CreateItemAsync(review, new PartitionKey(review.Pk));
                ShowReview(response.Value);
            }

            ShowProcessCompleted("CREATE ITEMS");

            return reviews;
        }

        private async Task GetReviews(string databaseId, string containerId)
        {
            ShowProcessStarted("GET ITEMS");

            var container = _cosmosClient.GetContainer(databaseId, containerId);

            var types = new[] { "Game", "Music", "Movie", "Book" };

            foreach (var type in types)
            {
                var query = $"SELECT * FROM c WHERE c.type = @type";
                var queryDefinition = new QueryDefinition(query).WithParameter("@type", type);

                await foreach (var review in container.GetItemQueryIterator<ReviewForCosmos>(queryDefinition))
                {
                    ShowReview(review);
                }

                Console.WriteLine();
            }

            ShowProcessCompleted("GET ITEMS");
        }

        private async Task GetReviewsHavingRatingGreaterThan(string databaseId, string containerId, string type, int minRating)
        {
            ShowProcessStarted($"GET {type.ToUpperInvariant()} ITEMS");

            var container = _cosmosClient.GetContainer(databaseId, containerId);

            var query = $"SELECT * FROM c WHERE c.type = @type AND c.rating > @rating";
            var queryDefinition = new QueryDefinition(query)
                .WithParameter("@type", type)
                .WithParameter("@rating", minRating);

            await foreach (var review in container.GetItemQueryIterator<ReviewForCosmos>(queryDefinition))
            {
                ShowReview(review);
            }

            ShowProcessCompleted($"GET {type.ToUpperInvariant()} ITEMS");
        }

        private async Task DeleteReviews(
            string databaseId,
            string containerId,
            IReadOnlyCollection<ReviewForCosmos> reviews)
        {
            ShowProcessStarted("DELETE ITEMS");

            var container = _cosmosClient.GetContainer(databaseId, containerId);

            foreach (var review in reviews)
            {
                await container.DeleteItemAsync<ReviewForCosmos>(review.Id, new PartitionKey(review.Pk));
                ShowReview(review);
            }

            ShowProcessCompleted("DELETE ITEMS");
        }

        private async Task DeleteContainer(string databaseId, string containerId)
        {
            ShowProcessStarted("DELETE CONTAINER");

            await _cosmosClient
                .GetDatabase(databaseId)
                .GetContainer(containerId)
                .DeleteContainerAsync();

            ShowProcessCompleted("DELETE CONTAINER");
        }

        private async Task DeleteDatabase(string databaseId)
        {
            ShowProcessStarted("DELETE DATABASE");

            await _cosmosClient.GetDatabase(databaseId).DeleteAsync();

            ShowProcessCompleted("DELETE DATABASE");
        }

        private static void ShowProcessStarted(string process)
        {
            AnsiConsole.MarkupLine($"\n\n[bold green3_1]{Emoji.Known.GreenCircle} '{process}' STARTED[/]");
        }

        private static void ShowProcessCompleted(string process)
        {
            AnsiConsole.MarkupLine($"\n[bold darkorange3_1]{Emoji.Known.OrangeCircle} '{process}' COMPLETED[/]");
        }

        private static void ShowReview(ReviewForCosmos review)
        {
            var hex = Color.FromConsoleColor(review.ToColor()).ToHex();
            AnsiConsole.Markup($"[bold #{hex}][[{review}]] [/]");
        }
    }
}
