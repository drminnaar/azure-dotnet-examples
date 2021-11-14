using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using FakeData;
using FakeData.Reviews;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace StorageQueue.Router.Consumer
{
    internal sealed class Program
    {
        internal static async Task Main()
        {
            var queueName = PromptQueueDetails();

            DisplayHeader(queueName);

            var provider = ConfigureServices();

            await Start(provider, queueName);
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

        private static async Task Start(IServiceProvider provider, string queueName)
        {
            var client = provider
                .GetRequiredService<QueueServiceClient>()
                .GetQueueClient(queueName);

            await client.CreateIfNotExistsAsync();

            while (true)
            {
                var messages = await client.ReceiveMessagesAsync();

                foreach (var message in messages.Value)
                {
                    var review = Review.FromBytes(message.Body.ToArray());
                    await client.DeleteMessageAsync(message.MessageId, message.PopReceipt);

                    DisplayOutput(review);
                }

                await Task.Delay(1000);
            }
        }

        private static void DisplayHeader(string queueName)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Text("AZURE STORAGE QUEUE", new Style(foreground: Color.Lime)).Centered());
            AnsiConsole.Write(new Text("ROUTER EXAMPLE", new Style(foreground: Color.Lime)).Centered());
            AnsiConsole.Write(new Text($"CONSUMER - {queueName.ToUpperInvariant()}", new Style(foreground: Color.Lime)).Centered());
            AnsiConsole.MarkupLine($"\n\n{Emoji.Known.GreenCircle}[bold lime] CONSUMER STARTED ...[/]");
            Console.WriteLine();
        }

        private static void DisplayOutput(Review review)
        {
            var hex = Color.FromConsoleColor(review.ToColor()).ToHex();
            AnsiConsole.Markup($"[bold #{hex}][[{review}]] [/]");
        }

        private static string PromptQueueDetails()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[lime]Queue Details[/]")
            {
                Alignment = Justify.Left
            });

            return AnsiConsole.Ask<string>("Enter [green]queue name[/]: ");
        }
    }
}
