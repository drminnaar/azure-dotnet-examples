using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using FakeData;
using FakeData.Reviews;
using Humanizer;
using Spectre.Console;

namespace StorageQueue.GettingStarted
{
    public sealed class AzureStorageQueueTest
    {
        private readonly QueueServiceClient _serviceClient;
        private readonly FakeEntityGeneratorBase<Review> _faker;

        public AzureStorageQueueTest(
            QueueServiceClient serviceClient,
            FakeEntityGeneratorBase<Review> faker)
        {
            _serviceClient = serviceClient ?? throw new ArgumentNullException(nameof(serviceClient));
            _faker = faker ?? throw new ArgumentNullException(nameof(faker));
        }

        internal async Task Start()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Text("AZURE STORAGE QUEUE", new Style(foreground: Color.Yellow2)).Centered());
            AnsiConsole.Write(new Text("GETTING STARTED", new Style(foreground: Color.Yellow2)).Centered());

            await CreateQueues();
            await SendMessages();
            await ReceiveMessages();
            await ReceiveAndDeleteMessages();
            await DeleteQueues();

            Console.WriteLine();
            AnsiConsole.Write(new Text("DEMO COMPLETE", new Style(foreground: Color.Yellow2)).Centered());
        }

        private async Task CreateQueues()
        {
            AnsiConsole.MarkupLine($"\n\n[bold yellow2]{Emoji.Known.GreenCircle} CREATE QUEUES STARTED ...[/]");
            Console.WriteLine();

            var queues = new List<QueueClient>();

            foreach (var queueType in FakeReviewGenerator.ReviewTypes)
            {
                queues.Add(await _serviceClient.CreateQueueAsync(queueType.ToLower()));
            }

            RenderQueuesAsTable(queues);
            AnsiConsole.MarkupLine($"\n[bold yellow2]{Emoji.Known.RedCircle} CREATE QUEUES COMPLETE...[/]");
        }

        private static void RenderQueuesAsTable(IReadOnlyCollection<QueueClient> queues)
        {
            var table = new Table() { Title = new TableTitle("Queues") }
                .RoundedBorder()
                .HeavyBorder()
                .AddColumn(nameof(QueueClient.AccountName))
                .AddColumn(nameof(QueueClient.Name))
                .AddColumn(nameof(QueueClient.CanGenerateSasUri))
                .AddColumn(nameof(QueueClient.MaxPeekableMessages))
                .AddColumn(nameof(QueueClient.MessageMaxBytes))
                .AddColumn(nameof(QueueClient.Uri));

            foreach (var queue in queues)
            {
                table.AddRow(
                    queue.AccountName,
                    queue.Name.Humanize(),
                    queue.CanGenerateSasUri.ToString(),
                    queue.MaxPeekableMessages.ToString(),
                    queue.MessageMaxBytes.Bytes().Humanize(),
                    queue.Uri.ToString());
            }

            Console.WriteLine();
            AnsiConsole.Write(table);
        }

        private async Task DeleteQueues()
        {
            AnsiConsole.MarkupLine($"\n\n[bold magenta1]{Emoji.Known.GreenCircle} DELETE QUEUES STARTED ...[/]");
            Console.WriteLine();

            var queues = await ListQueues();

            foreach (var queue in queues)
            {
                await _serviceClient.DeleteQueueAsync(queue.Name);
                AnsiConsole.MarkupLine($"[bold magenta1]'{queue.Name}' queue deleted[/]");
            }

            AnsiConsole.MarkupLine($"\n[bold magenta1]{Emoji.Known.RedCircle} DELETE QUEUES COMPLETED...[/]");
        }

        private async Task<List<QueueItem>> ListQueues()
        {
            var queuePages = _serviceClient
                .GetQueuesAsync()
                .AsPages()
                .GetAsyncEnumerator();

            var queues = new List<QueueItem>();

            while (await queuePages.MoveNextAsync())
            {
                var page = queuePages.Current;
                if (page.Values.Any())
                    queues.AddRange(page.Values);
            }

            return queues;
        }

        private async Task ReceiveMessages()
        {
            AnsiConsole.MarkupLine($"\n\n[bold cyan1]{Emoji.Known.GreenCircle} RECEIVING MESSAGES STARTED...[/]");
            Console.WriteLine();

            var queues = await ListQueues();

            var reviews = new List<Review>();

            foreach (var queue in queues)
            {
                var queueClient = _serviceClient.GetQueueClient(queue.Name);
                var messages = await queueClient.ReceiveMessagesAsync(30);

                foreach (var message in messages.Value)
                    reviews.Add(Review.FromBytes(message.Body.ToArray()));
            }

            RenderReceivedMessagesAsTableTree(reviews);

            AnsiConsole.MarkupLine($"\n[bold cyan1]{Emoji.Known.RedCircle} RECEIVING MESSAGES COMPLETED...[/]");
        }

        private async Task ReceiveAndDeleteMessages()
        {
            AnsiConsole.MarkupLine($"\n\n[bold khaki1]{Emoji.Known.GreenCircle} RECEIVING AND DELETING MESSAGES STARTED...[/]");
            Console.WriteLine();

            var queues = await ListQueues();

            foreach (var queue in queues)
            {
                var queueClient = _serviceClient.GetQueueClient(queue.Name);
                var messages = await queueClient.ReceiveMessagesAsync(maxMessages: 30);

                foreach (var message in messages.Value)
                {
                    await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                    AnsiConsole.MarkupLine($"[bold khaki1]All messages in '{queue.Name}' queue deleted[/]");
                }
            }

            AnsiConsole.MarkupLine($"\n[bold khaki1]{Emoji.Known.RedCircle} RECEIVING AND DELETING MESSAGES COMPLETED...[/]");
        }

        private static void RenderReceivedMessagesAsTableTree(IReadOnlyCollection<Review> reviews)
        {
            var root = new Tree("[bold cyan1]Azure Storage Queues[/]").Guide(TreeGuide.BoldLine);

            var groups = reviews
                .ToList()
                .GroupBy(review => review.Type)
                .ToList();

            foreach (var group in groups)
            {
                var queueNode = root.AddNode($"[cyan1]{group.Key}[/]");

                var table = new Table()
                    .RoundedBorder()
                    .AddColumn(nameof(Review.Title))
                    .AddColumn(nameof(Review.ReviewedBy))
                    .AddColumn(nameof(Review.Rating));

                foreach (var review in group)
                    table.AddRow(review.Title, review.ReviewedBy, review.Rating.ToString());

                queueNode.AddNode(table);
            }

            AnsiConsole.Write(root);
        }

        private async Task SendMessages()
        {
            AnsiConsole.MarkupLine($"\n\n[bold orange1]{Emoji.Known.GreenCircle} SENDING MESSAGES STARTED ...[/]");
            Console.WriteLine();

            var reviews = _faker.GenerateFakes(20);
            var receipts = new List<SendReceipt>();

            foreach (var review in reviews)
            {
                await Task.Delay(300);

                var queueClient = _serviceClient.GetQueueClient(review.Type.ToLowerInvariant());

                await queueClient.CreateIfNotExistsAsync();

                receipts.Add(await queueClient.SendMessageAsync(review.ToJson()));
            }

            RenderSentReceiptsAsTable(receipts);
            AnsiConsole.MarkupLine($"\n[bold orange1]{Emoji.Known.RedCircle} SENDING MESSAGES COMPLETED ...[/]");
            Console.WriteLine();
        }

        private static void RenderSentReceiptsAsTable(IReadOnlyCollection<SendReceipt> receipts)
        {
            var table = new Table() { Title = new TableTitle("Send Receipts") }
                .RoundedBorder()
                .AddColumn(nameof(SendReceipt.MessageId))
                .AddColumn(nameof(SendReceipt.PopReceipt))
                .AddColumn(nameof(SendReceipt.ExpirationTime))
                .AddColumn(nameof(SendReceipt.InsertionTime))
                .AddColumn(nameof(SendReceipt.TimeNextVisible));

            foreach (var receipt in receipts)
            {
                table.AddRow(
                    receipt.MessageId,
                    receipt.PopReceipt,
                    receipt.ExpirationTime.Humanize(),
                    receipt.InsertionTime.Humanize(),
                    receipt.TimeNextVisible.Humanize());
            }

            Console.WriteLine();
            AnsiConsole.Write(table);
        }
    }
}
