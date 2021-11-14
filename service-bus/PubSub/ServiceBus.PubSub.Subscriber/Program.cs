using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace ServiceBus.PubSub.Subscriber
{
    internal sealed class Program
    {
        internal static async Task Main()
        {
            PromptSubscriptionDetails(out var topicName, out var subscriberName);

            DisplayHeader(subscriberName);

            await Start(ConfigureServices(), topicName, subscriberName);
        }

        private static async Task Start(ServiceProvider provider, string topicName, string subscriberName)
        {
            // create subscription
            var subscriptionProperties = await provider
                .GetRequiredService<ReviewSubscriptionBuilder>()
                .Build(topicName, subscriberName);

            // subscribe
            await provider
                .GetRequiredService<ConsoleReviewSubscriber>()
                .Subscribe(subscriptionProperties);
        }

        private static ServiceProvider ConfigureServices()
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
                .AddSingleton(provider =>
                {
                    return new ReviewSubscriptionBuilder(
                        provider.GetRequiredService<ServiceBusAdministrationClient>());
                })
                .AddSingleton(provider =>
                {
                    return new ConsoleReviewSubscriber(
                        provider.GetRequiredService<ServiceBusClient>());
                })
                .BuildServiceProvider();
        }

        private static void DisplayHeader(string subscriberName)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Text($"AZURE SERVICE BUS", new Style(foreground: Color.LightSalmon1)).Centered());
            AnsiConsole.Write(new Text($"PUB SUB", new Style(foreground: Color.LightSalmon1)).Centered());
            AnsiConsole.Write(new Text($"SUBSCRIBER - {subscriberName}", new Style(foreground: Color.LightSalmon1)).Centered());
            Console.WriteLine();
        }

        private static void PromptSubscriptionDetails(
            out string topicName,
            out string subscriberName)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[lime]Subscription Details[/]")
            {
                Alignment = Justify.Left
            });

            topicName = AnsiConsole.Ask<string>("Enter [green]topic name[/]: ");

            subscriberName = AnsiConsole.Ask<string>("Enter [green]subscriber name[/]: ");
        }
    }
}
