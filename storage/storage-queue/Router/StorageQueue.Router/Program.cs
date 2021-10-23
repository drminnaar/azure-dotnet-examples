using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using FakeData;
using FakeData.Reviews;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace StorageQueue.Router
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

            var serviceClient = provider.GetRequiredService<QueueServiceClient>();

            var client = serviceClient.GetQueueClient(QueueName);
            await client.CreateIfNotExistsAsync();

            // start receiving messages
            while (true)
            {
                var messages = await client.ReceiveMessagesAsync(maxMessages: 30);

                foreach (var message in messages.Value)
                {
                    var review = Review.FromBytes(message.Body.ToArray());

                    // get/create client to routed queue and send message
                    var router = serviceClient.GetQueueClient($"{review.Type.ToLowerInvariant()}-{QueueName}");
                    await router.CreateIfNotExistsAsync();
                    await router.SendMessageAsync(review.ToJson());
                    await client.DeleteMessageAsync(message.MessageId, message.PopReceipt);

                    DisplayOutput(review);
                }

                await Task.Delay(1000);
            }
        }

        private static void DisplayHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Text("AZURE STORAGE QUEUE", new Style(foreground: Color.OrangeRed1)).Centered());
            AnsiConsole.Write(new Text("ROUTER EXAMPLE", new Style(foreground: Color.OrangeRed1)).Centered());
            AnsiConsole.Write(new Text("ROUTER", new Style(foreground: Color.OrangeRed1)).Centered());
            AnsiConsole.MarkupLine($"\n\n{Emoji.Known.GreenCircle}[bold orangered1] ROUTER STARTED ...[/]");
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
