using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using FakeData;
using FakeData.Reviews;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StorageQueue.GettingStarted
{
    internal sealed class Program
    {
        internal static async Task Main()
        {
            var provider = ConfigureServices();
            var faker = provider.GetRequiredService<FakeEntityGeneratorBase<Review>>();
            var serviceClient = provider.GetRequiredService<QueueServiceClient>();

            // Start Test
            await new AzureStorageQueueTest(serviceClient, faker).Start();
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
    }
}
