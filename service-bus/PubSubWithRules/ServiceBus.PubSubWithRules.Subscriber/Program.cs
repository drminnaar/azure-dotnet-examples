using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using FakeData.Reviews;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace ServiceBus.PubSubWithRules.Subscriber
{
    internal sealed class Program
    {
        internal static async Task Main()
        {
            AnsiConsole.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose [green]subscriber type[/]?")
                    .AddChoices(new[] {
                        "1 - Review Type Subsciption (filter by review type using correlation rule filter)",
                        "2 - Rating Subsciption (filter by review rating using Sql rule filter)",
                        "3 - Game Rating Subsciption (filter by game review rating using Sql rule filter)",
                    }));

            if (choice.StartsWith("1"))
            {
                await StartCorrelationFilterRuleDemo();
            }
            else if (choice.StartsWith("2"))
            {
                await StartSqlFilterRuleDemo();
            }
            else
            {
                await StartSqlFilterRuleDemo2();
            }
        }

        private static async Task StartCorrelationFilterRuleDemo()
        {
            var connectionString = GetConnectionString();
            var sbAdminClient = new ServiceBusAdministrationClient(connectionString);
            var topicName = "reviews";
            var subscriptionName = "game-reviews";

            DisplayHeader(subscriptionName);

            // Create Topic
            if (!await sbAdminClient.TopicExistsAsync(topicName))
                await sbAdminClient.CreateTopicAsync(topicName);

            // Create Subscription
            if (await sbAdminClient.SubscriptionExistsAsync(topicName, subscriptionName))
                await sbAdminClient.DeleteSubscriptionAsync(topicName, subscriptionName);

            var subscriptionProperties = await sbAdminClient.CreateSubscriptionAsync(
                new CreateSubscriptionOptions(topicName, subscriptionName));

            // Configure Rules
            await sbAdminClient.DeleteRuleAsync(topicName, subscriptionName, RuleProperties.DefaultRuleName);

            var ruleName = "game-subject-correlation-rule";

            if (!await sbAdminClient.RuleExistsAsync(topicName, subscriptionName, ruleName))
            {
                await sbAdminClient.CreateRuleAsync(topicName, subscriptionName, new CreateRuleOptions
                {
                    Action = null,
                    Filter = new CorrelationRuleFilter
                    {
                        Subject = "game"
                    },
                    Name = ruleName
                });
            }

            // Subscribe
            var sbClient = new ServiceBusClient(connectionString);
            var processor = sbClient.CreateProcessor(
                subscriptionProperties.Value.TopicName,
                subscriptionProperties.Value.SubscriptionName);

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

        private static async Task StartSqlFilterRuleDemo()
        {
            var connectionString = GetConnectionString();
            var sbAdminClient = new ServiceBusAdministrationClient(connectionString);
            var topicName = "reviews";
            var subscriptionName = "highest-rated-reviews";

            DisplayHeader(subscriptionName);

            // Create Topic
            if (!await sbAdminClient.TopicExistsAsync(topicName))
                await sbAdminClient.CreateTopicAsync(topicName);

            // Create Subscription
            if (await sbAdminClient.SubscriptionExistsAsync(topicName, subscriptionName))
                await sbAdminClient.DeleteSubscriptionAsync(topicName, subscriptionName);

            var subscriptionProperties = await sbAdminClient.CreateSubscriptionAsync(
                new CreateSubscriptionOptions(topicName, subscriptionName));

            // Configure Rules
            await sbAdminClient.DeleteRuleAsync(topicName, subscriptionName, RuleProperties.DefaultRuleName);

            var ruleName = "game-rating-sql-rule";

            var rating = AnsiConsole.Prompt(
                new TextPrompt<int>("Enter [green]rating (1-10)[/]: ")
                    .Validate(r =>
                    {
                        return r switch
                        {
                            < 1 => ValidationResult.Error("[indianred1_1]Too low[/]"),
                            > 10 => ValidationResult.Error("[indianred1_1]Too high[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));

            if (!await sbAdminClient.RuleExistsAsync(topicName, subscriptionName, ruleName))
            {
                await sbAdminClient.CreateRuleAsync(topicName, subscriptionName, new CreateRuleOptions
                {
                    Filter = new SqlRuleFilter($"rating >= {rating}"),
                    Name = ruleName
                });
            }

            // Subscribe
            var sbClient = new ServiceBusClient(connectionString);
            var processor = sbClient.CreateProcessor(
                subscriptionProperties.Value.TopicName,
                subscriptionProperties.Value.SubscriptionName);

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

        private static async Task StartSqlFilterRuleDemo2()
        {
            var connectionString = GetConnectionString();
            var sbAdminClient = new ServiceBusAdministrationClient(connectionString);
            var topicName = "reviews";
            var subscriptionName = "highest-rated-game-reviews";

            DisplayHeader(subscriptionName);

            // Create Topic
            if (!await sbAdminClient.TopicExistsAsync(topicName))
                await sbAdminClient.CreateTopicAsync(topicName);

            // Create Subscription
            if (await sbAdminClient.SubscriptionExistsAsync(topicName, subscriptionName))
                await sbAdminClient.DeleteSubscriptionAsync(topicName, subscriptionName);

            var subscriptionProperties = await sbAdminClient.CreateSubscriptionAsync(
                new CreateSubscriptionOptions(topicName, subscriptionName));

            // Configure Rules
            await sbAdminClient.DeleteRuleAsync(topicName, subscriptionName, RuleProperties.DefaultRuleName);

            var ruleName = "game-type-rating-sql-rule";

            var rating = AnsiConsole.Prompt(
                new TextPrompt<int>("Enter [green]rating (1-10)[/]: ")
                    .Validate(r =>
                    {
                        return r switch
                        {
                            < 1 => ValidationResult.Error("[indianred1_1]Too low[/]"),
                            > 10 => ValidationResult.Error("[indianred1_1]Too high[/]"),
                            _ => ValidationResult.Success(),
                        };
                    }));

            if (!await sbAdminClient.RuleExistsAsync(topicName, subscriptionName, ruleName))
            {
                await sbAdminClient.CreateRuleAsync(topicName, subscriptionName, new CreateRuleOptions
                {
                    Filter = new SqlRuleFilter($"rating >= {rating} AND type = 'Game'"),
                    Name = ruleName
                });
            }

            // Subscribe
            var sbClient = new ServiceBusClient(connectionString);
            var processor = sbClient.CreateProcessor(
                subscriptionProperties.Value.TopicName,
                subscriptionProperties.Value.SubscriptionName);

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

        private static string GetConnectionString() => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<Program>(optional: false, reloadOnChange: true)
            .Build()
            .GetConnectionString("ServiceBus");

        private static async Task ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            var review = Review.FromBytes(arg.Message.Body.ToArray());
            var hex = Color.FromConsoleColor(review.ToColor()).ToHex();
            AnsiConsole.Markup($"[bold #{hex}][[{review}]][/] ");
            await arg.CompleteMessageAsync(arg.Message);
        }

        private static Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            AnsiConsole.MarkupLine($"\n[bold deeppink4_2][[SUBSCRIBER_ERROR]]::{arg.Exception.Message}[/]");
            return Task.CompletedTask;
        }

        private static void DisplayHeader(string subscription)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Text($"AZURE SERVICE BUS", new Style(foreground: Color.LightSteelBlue)).Centered());
            AnsiConsole.Write(new Text($"PUB SUB", new Style(foreground: Color.LightSteelBlue)).Centered());
            AnsiConsole.Write(new Text($"SUBSCRIPTION - {subscription}", new Style(foreground: Color.LightSteelBlue)).Centered());
            Console.WriteLine();
        }
    }
}
