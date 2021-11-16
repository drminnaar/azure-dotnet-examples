using System;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using FakeData;
using FakeData.Weather;
using Spectre.Console;

namespace TableStorage.GettingStarted
{
    public sealed class TableStorageTest
    {
        private readonly TableClient _tableClient;
        private readonly TableServiceClient _tableServiceClient;
        private readonly FakeEntityGeneratorBase<Forecast> _faker;

        public TableStorageTest(TableClient tableClient, TableServiceClient tableServiceClient, FakeEntityGeneratorBase<Forecast> faker)
        {
            _tableClient = tableClient ?? throw new ArgumentNullException(nameof(tableClient));
            _tableServiceClient = tableServiceClient ?? throw new ArgumentNullException(nameof(tableServiceClient));
            _faker = faker ?? throw new ArgumentNullException(nameof(faker));
        }

        public async Task Run()
        {
            await CreateTableIfNotExists();
            await AddEntities();
            await QueryEntities();
            await DeleteTable();
        }

        private async Task CreateTableIfNotExists()
        {
            ShowProcessStarted(nameof(CreateTableIfNotExists));
            TableItem result = await _tableClient.CreateIfNotExistsAsync();
            ShowProcessCompleted(nameof(CreateTableIfNotExists));
        }

        private async Task AddEntities()
        {
            ShowProcessStarted(nameof(AddEntities));

            var forecasts = _faker.GenerateFakes(1000);

            foreach (var forecast in forecasts)
            {
                await _tableClient.AddEntityAsync(new ForecastTableEntity(forecast));
            }

            ShowProcessCompleted(nameof(AddEntities));
        }

        private async Task QueryEntities()
        {
            ShowProcessStarted(nameof(QueryEntities));

            var forecastPages = _tableClient.QueryAsync<ForecastTableEntity>(forecast => forecast.PartitionKey == "London");

            await foreach (var forecastPage in forecastPages.AsPages())
            {
                foreach (var forecast in forecastPage.Values)
                {
                    ShowForecast(forecast);
                }
            }

            ShowProcessCompleted(nameof(QueryEntities));
        }

        private async Task DeleteTable()
        {
            ShowProcessStarted(nameof(DeleteTable));
            await _tableClient.DeleteAsync();
            ShowProcessCompleted(nameof(DeleteTable));
        }

        private static void ShowProcessStarted(string process)
        {
            AnsiConsole.MarkupLine($"\n\n[bold green3_1]{Emoji.Known.GreenCircle} '{process.ToUpperInvariant()}' STARTED[/]");
        }

        private static void ShowProcessCompleted(string process)
        {
            AnsiConsole.MarkupLine($"\n[bold red3_1]{Emoji.Known.RedCircle} '{process.ToUpperInvariant()}' COMPLETED[/]");
        }

        private static void ShowForecast(ForecastTableEntity forecast)
        {
            var hex = Color.FromConsoleColor(forecast.ToColor()).ToHex();
            AnsiConsole.MarkupLine($"[bold #{hex}][[City: {forecast.City}, High: {forecast.High}, Low: {forecast.Low}]] [/]");
        }
    }
}
