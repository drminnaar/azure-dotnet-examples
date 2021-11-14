using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using FakeData;
using FakeData.Reviews;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace StorageQueue.OneWayMessaging.Consumer
{
    internal sealed class Program
    {
        internal static async Task Main()
        {
            DisplayHeader();

            var provider = ConfigureServices();

            const string QueueName = "reviews";

            await Start(provider, QueueName);
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

            // start receiving messages
            while (true)
            {
                var messages = await client.ReceiveMessagesAsync();

                foreach (var message in messages.Value)
                {
                    var review = Review.FromBytes(message.Body.ToArray());
                    DisplayOutput(review);
                    await client.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                }

                await Task.Delay(1000);
            }
        }

        private static void DisplayHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Text("AZURE STORAGE QUEUE", new Style(foreground: Color.Magenta1)).Centered());
            AnsiConsole.Write(new Text("ONE WAY MESSAGING", new Style(foreground: Color.Magenta1)).Centered());
            AnsiConsole.Write(new Text("CONSUMER", new Style(foreground: Color.Magenta1)).Centered());
            AnsiConsole.MarkupLine($"\n\n{Emoji.Known.GreenCircle}[bold magenta1] CONSUMER STARTED ...[/]");
            Console.WriteLine();
        }

        private static void DisplayOutput(Review review)
        {
            var hex = Color.FromConsoleColor(review.ToColor()).ToHex();
            AnsiConsole.Markup($"[bold #{hex}][[{review}]] [/]");
        }
    }
}
