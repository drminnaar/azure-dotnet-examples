using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using FakeData;
using FakeData.Reviews;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace StorageQueue.Router.Producer
{
    internal sealed class Program
    {
        internal static async Task Main()
        {
            DisplayHeader();

            var provider = ConfigureServices();

            await Start(provider);
        }

        private static IServiceProvider ConfigureServices()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>(optional: false, reloadOnChange: true)
                .Build();

            return new ServiceCollection()
                .AddSingleton(new QueueServiceClient(configuration.GetConnectionString("StorageQueue")))
                .AddSingleton<FakeEntityGeneratorBase<Review>>(new FakeReviewGenerator())
                .BuildServiceProvider();
        }

        private static async Task Start(IServiceProvider provider)
        {
            const string QueueName = "reviews";

            var faker = provider.GetRequiredService<FakeEntityGeneratorBase<Review>>();

            var client = provider
                .GetRequiredService<QueueServiceClient>()
                .GetQueueClient(QueueName);

            await client.CreateIfNotExistsAsync();

            while (true)
            {
                var review = faker.GenerateFakes(1).First();
                await client.SendMessageAsync(review.ToJson());
                DisplayOutput(review);
                await Task.Delay(1000);
            }
        }

        private static void DisplayHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Text("AZURE STORAGE QUEUE", new Style(foreground: Color.Yellow2)).Centered());
            AnsiConsole.Write(new Text("ROUTER EXAMPLE", new Style(foreground: Color.Yellow2)).Centered());
            AnsiConsole.Write(new Text("PRODUCER", new Style(foreground: Color.Yellow2)).Centered());
            AnsiConsole.MarkupLine($"\n\n{Emoji.Known.GreenCircle}[bold yellow2] PRODUCER STARTED ...[/]");
            Console.WriteLine();
        }

        private static void DisplayOutput(Review review)
        {
            var hex = Color.FromConsoleColor(review.ToColor()).ToHex();
            var type = review.Type.ToUpperInvariant();
            AnsiConsole.Markup($"[bold #{hex}][[{review}]] [/]");
        }
    }
}
