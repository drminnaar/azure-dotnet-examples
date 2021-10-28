using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using FakeData.Reviews;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace ServiceBus.OneWayMessaging.Consumer
{
    internal sealed class Program
    {
        internal static async Task Main()
        {
            DisplayHeader();
            await Start(Configure());
        }

        private static ServiceProvider Configure()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>(optional: false, reloadOnChange: true)
                .Build();

            return new ServiceCollection()
                .AddSingleton(new ServiceBusClient(configuration.GetConnectionString("ServiceBus")))
                .BuildServiceProvider();
        }

        private static async Task Start(ServiceProvider provider)
        {
            var client = provider.GetRequiredService<ServiceBusClient>();

            const string QueueName = "reviews";

            var options = new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 1,
                PrefetchCount = 2,
                SubQueue = SubQueue.None,
                ReceiveMode = ServiceBusReceiveMode.PeekLock
            };

            while (true)
            {
                await using var processor = client.CreateProcessor(QueueName, options);
                processor.ProcessMessageAsync += ProcessMessageAsync;
                processor.ProcessErrorAsync += ProcessErrorAsync;

                // start processing
                await processor.StartProcessingAsync();
                AnsiConsole.MarkupLine($"\n[bold green3_1]{Emoji.Known.GreenCircle} CONSUMER PROCESSING STARTED (press any key to stop processing)[/]");
                Console.WriteLine();
                Console.ReadKey();

                // stop processing
                AnsiConsole.MarkupLine($"\n[bold yellow2]{Emoji.Known.YellowCircle} CONSUMER STOPPING...[/]");
                await processor.StopProcessingAsync();
                AnsiConsole.MarkupLine($"\n[bold red3_1]{Emoji.Known.RedCircle} CONSUMER STOPPED[/]");
                break;
            }
        }

        private static async Task ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            var review = Review.FromBytes(arg.Message.Body.ToArray());
            DisplayOutput(review);
            await arg.CompleteMessageAsync(arg.Message);
        }

        private static Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            AnsiConsole.MarkupLine($"\n[bold deeppink4_2][[CONSUMER_ERROR]]::{arg.Exception.Message}[/]");
            return Task.CompletedTask;
        }

        private static void DisplayHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Text($"AZURE SERVICE BUS", new Style(foreground: Color.LightSalmon1)).Centered());
            AnsiConsole.Write(new Text($"ONE WAY MESSAGING", new Style(foreground: Color.LightSalmon1)).Centered());
            AnsiConsole.Write(new Text($"CONSUMER", new Style(foreground: Color.LightSalmon1)).Centered());
            Console.WriteLine();
        }

        private static void DisplayOutput(Review review)
        {
            var hex = Color.FromConsoleColor(review.ToColor()).ToHex();
            var type = review.Type.ToUpperInvariant();
            AnsiConsole.Markup($"[bold #{hex}] [[{type}]] [/]");
        }
    }
}
