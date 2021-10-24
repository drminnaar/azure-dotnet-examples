using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Cosmos;
using FakeData;
using FakeData.Reviews;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace CosmosDb.GettingStarted
{
    internal sealed class Program
    {
        internal static async Task Main()
        {
            DisplayHeader();

            try
            {
                var test = ConfigureServices().GetRequiredService<CosmosDbTest>();
                await test.Run();
            }
            catch (Exception error)
            {
                LogError(error);
            }

            Console.ReadLine();
        }

        private static IServiceProvider ConfigureServices()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>(optional: false, reloadOnChange: true)
                .Build();

            return new ServiceCollection()
                .AddSingleton(new CosmosClient(configuration.GetConnectionString("CosmosDb")))
                .AddSingleton<FakeEntityGeneratorBase<ReviewForCosmos>>(new FakeReviewForCosmosGenerator())
                .AddSingleton(provider => new CosmosDbTest(
                    provider.GetRequiredService<CosmosClient>(),
                    provider.GetRequiredService<FakeEntityGeneratorBase<ReviewForCosmos>>()))
                .BuildServiceProvider();
        }

        private static void DisplayHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Text($"AZURE COSMOSDB", new Style(foreground: Color.LightSalmon1)).Centered());
            AnsiConsole.Write(new Text($"GETTING STARTED", new Style(foreground: Color.LightSalmon1)).Centered());
            Console.WriteLine();
        }

        private static void LogError(Exception error)
        {
            AnsiConsole.Write(new Text(
                $"{Emoji.Known.Bomb} ERROR: {error.Message}",
                new Style(foreground: Color.Red1,
                decoration: Decoration.Bold)));
        }
    }
}
