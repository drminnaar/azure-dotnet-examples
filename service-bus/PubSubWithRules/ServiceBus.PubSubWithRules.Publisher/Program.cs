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

namespace ServiceBus.PubSubWithRules.Publisher
{
    internal sealed class Program
    {
        internal static async Task Main()
        {
            DisplayHeader();

            await Start(ConfigureServices(), topicName: "reviews");
        }

        private static IServiceProvider ConfigureServices()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>(optional: false, reloadOnChange: true)
                .Build();

            const string ConnectionStringName = "ServiceBus";

            return new ServiceCollection()
                .AddSingleton(new ServiceBusClient(configuration.GetConnectionString(ConnectionStringName)))
                .AddSingleton(new ServiceBusAdministrationClient(configuration.GetConnectionString(ConnectionStringName)))
                .AddSingleton<IServiceBusMessageBuilder>(new ServiceBusMessageBuilder())
                .AddSingleton<FakeEntityGeneratorBase<Review>>(new FakeReviewGenerator())
                .BuildServiceProvider();
        }

        private static async Task Start(IServiceProvider provider, string topicName)
        {
            // define required services
            var fakes = provider.GetRequiredService<FakeEntityGeneratorBase<Review>>();
            var client = provider.GetRequiredService<ServiceBusClient>();
            var adminClient = provider.GetRequiredService<ServiceBusAdministrationClient>();
            var messageBuilder = provider.GetRequiredService<IServiceBusMessageBuilder>();

            // create topic if not exists
            if (!await adminClient.TopicExistsAsync(topicName))
                await adminClient.CreateTopicAsync(new CreateTopicOptions(topicName));

            await using var sender = client.CreateSender(topicName);

            // generate messages (fake reviews)
            while (true)
            {
                var review = fakes.GenerateFakes(1).First();

                try
                {
                    var message = messageBuilder
                        .New()
                        .SetMessage(review)
                        .SetSubject(review.Type)
                        .AddApplicationProperty(nameof(Review.Type), review.Type)
                        .AddApplicationProperty(nameof(Review.Rating), review.Rating)
                        .Build();

                    await sender.SendMessageAsync(message);
                }
                catch (ServiceBusException exception)
                {
                    AnsiConsole.MarkupLine($"[bold indianred1][[PUBLISHER_ERROR]]:: {exception.Message}[/]");
                }

                DisplayOutput(review);
                await Task.Delay(1000);
            }
        }

        private static void DisplayHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Text("AZURE SERVICE BUS", new Style(foreground: Color.Yellow2)).Centered());
            AnsiConsole.Write(new Text("PUB SUB", new Style(foreground: Color.Yellow2)).Centered());
            AnsiConsole.Write(new Text("PUBLISHER", new Style(foreground: Color.Yellow2)).Centered());
            Console.WriteLine();
            Console.WriteLine("Press any key to start");
            Console.WriteLine();
            Console.ReadKey();
            AnsiConsole.MarkupLine($"\n[bold green3_1]{Emoji.Known.GreenCircle} PUBLISHER STARTED[/]");
            Console.WriteLine();
        }

        private static void DisplayOutput(Review review)
        {
            var hex = Color.FromConsoleColor(review.ToColor()).ToHex();
            AnsiConsole.Markup($"[bold #{hex}] [[{review}]] [/]");
        }
    }
}
