using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using FakeData.Reviews;
using Spectre.Console;

namespace ServiceBus.PubSub.Subscriber
{
    public sealed class ConsoleReviewSubscriber
    {
        private readonly ServiceBusClient _client;

        public ConsoleReviewSubscriber(ServiceBusClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task Subscribe(SubscriptionProperties subscriptionProperties)
        {
            await using var processor = _client.CreateProcessor(
                subscriptionProperties.TopicName,
                subscriptionProperties.SubscriptionName);

            processor.ProcessMessageAsync += ProcessMessageAsync;
            processor.ProcessErrorAsync += ProcessErrorAsync;

            // start processing
            await processor.StartProcessingAsync();
            AnsiConsole.MarkupLine($"\n[bold green3_1]{Emoji.Known.GreenCircle} SUBSCRIBER PROCESSING STARTED (press any key to stop processing)[/]");
            Console.WriteLine();
            Console.ReadKey();

            // stop processing
            AnsiConsole.MarkupLine($"\n[bold yellow2]{Emoji.Known.YellowCircle} SUBSCRIBER STOPPING...[/]");
            await processor.StopProcessingAsync();
            AnsiConsole.MarkupLine($"\n[bold red3_1]{Emoji.Known.RedCircle} SUBSCRIBER STOPPED[/]");
            processor.ProcessMessageAsync -= ProcessMessageAsync;
            processor.ProcessErrorAsync -= ProcessErrorAsync;
        }

        private static async Task ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            var review = Review.FromBytes(arg.Message.Body.ToArray());
            DisplayOutput(review);
            await arg.CompleteMessageAsync(arg.Message);
        }

        private static Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            AnsiConsole.MarkupLine($"\n[bold deeppink4_2][[SUBSCRIBER_ERROR]]::{arg.Exception.Message}[/]");
            return Task.CompletedTask;
        }

        private static void DisplayOutput(Review review)
        {
            var hex = Color.FromConsoleColor(review.ToColor()).ToHex();
            AnsiConsole.Markup($"[bold #{hex}][[{review.Type}]][/] ");
        }
    }
}
