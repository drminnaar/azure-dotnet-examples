using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using FakeData;
using FakeData.Reviews;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace ServiceBus.OneWayMessaging.Producer
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
                .AddSingleton<FakeEntityGeneratorBase<Review>>(new FakeReviewGenerator())
                .AddSingleton(new ServiceBusAdministrationClient(configuration.GetConnectionString("ServiceBus")))
                .AddSingleton(new ServiceBusClient(configuration.GetConnectionString("ServiceBus")))
                .AddSingleton<IServiceBusMessageBuilder, ServiceBusMessageBuilder>()
                .BuildServiceProvider();
        }

        private static async Task Start(ServiceProvider provider)
        {
            var fakes = provider.GetRequiredService<FakeEntityGeneratorBase<Review>>();
            var messageBuilder = provider.GetRequiredService<IServiceBusMessageBuilder>();
            var client = provider.GetRequiredService<ServiceBusClient>();
            var adminClient = provider.GetRequiredService<ServiceBusAdministrationClient>();

            const string QueueName = "reviews";

            if (!await adminClient.QueueExistsAsync(QueueName))
                await adminClient.CreateQueueAsync(new CreateQueueOptions(QueueName));

            await using var sender = client.CreateSender(QueueName);

            while (true)
            {
                var review = fakes.GenerateFakes(1).First();

                try
                {
                    var message = messageBuilder
                        .New()
                        .SetMessage(review)
                        .Build();

                    await sender.SendMessageAsync(message);
                }
                catch (ServiceBusException exception)
                {
                    AnsiConsole.MarkupLine($"[bold deeppink4_2][[PRODUCER_ERROR]]:: {exception.Message}[/]");
                }

                DisplayOutput(review);
                await Task.Delay(1000);
            }
        }

        private static void DisplayHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Text("AZURE SERVICE BUS", new Style(foreground: Color.Yellow2)).Centered());
            AnsiConsole.Write(new Text("ONE WAY MESSAGING", new Style(foreground: Color.Yellow2)).Centered());
            AnsiConsole.Write(new Text("PRODUCER", new Style(foreground: Color.Yellow2)).Centered());
            Console.WriteLine();
            AnsiConsole.MarkupLine($"\n[bold green3_1]{Emoji.Known.GreenCircle} PRODUCER STARTED[/]");
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
