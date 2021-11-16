using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using FakeData;
using FakeData.Reviews;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace StorageContainer.GettingStarted
{
    internal sealed class Program
    {
        internal static async Task Main()
        {
            var provider = ConfigureServices();
            var serviceClient = provider.GetRequiredService<BlobServiceClient>();

            DisplayHeader();

            // create file
            Display("CREATE FILE");
            var file = await CreateFile(provider);

            // create container
            Display("CREATE CONTAINER");
            var containerName = $"getting-started-{Guid.NewGuid()}";
            var containerClient = await serviceClient.CreateBlobContainerAsync(containerName);

            // upload file
            Display("UPLOAD FILE");
            var blobClient = containerClient.Value.GetBlobClient(file.FileName);
            await blobClient.UploadAsync(path: file.Path, overwrite: true);

            // get file
            Display("GET FILE CONTENT");
            blobClient = containerClient.Value.GetBlobClient(file.FileName);
            using var stream = new StreamReader(await blobClient.OpenReadAsync());
            var fileContentFromBlob = await stream.ReadToEndAsync();
            Console.WriteLine($"\n{fileContentFromBlob}");

            // download file
            Display("DOWNLOAD FILE");
            blobClient = containerClient.Value.GetBlobClient(file.FileName);
            await blobClient.DownloadToAsync(file.DownloadPath);

            // delete file
            Display("DELETE FILE");
            blobClient = containerClient.Value.GetBlobClient(file.FileName);
            await blobClient.DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.None);

            // delete container
            Display("DELETE CONTAINER");
            await containerClient.Value.DeleteIfExistsAsync();

            // delete data folder
            Directory.Delete(file.Directory, true);

            Console.WriteLine();
            AnsiConsole.Write(new Text("\nDEMO COMPLETE", new Style(foreground: Color.Yellow2)).Centered());
        }

        private static void DisplayHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Text("AZURE STORAGE BLOB", new Style(foreground: Color.Yellow2)).Centered());
            AnsiConsole.Write(new Text("GETTING STARTED", new Style(foreground: Color.Yellow2)).Centered());
            Console.WriteLine();
        }

        private static async Task<dynamic> CreateFile(IServiceProvider provider)
        {
            var faker = provider.GetRequiredService<FakeEntityGeneratorBase<Review>>();

            var dataDirectory = $"./data-{Guid.NewGuid()}";

            if (!Directory.Exists(dataDirectory))
                Directory.CreateDirectory(dataDirectory);

            var (fileName, fileContent) = faker.GenerateFakes(10).ToRandomFile();
            var localFilePath = Path.Combine(dataDirectory, fileName);
            await File.WriteAllTextAsync(localFilePath, fileContent);

            return new
            {
                Directory = dataDirectory,
                FileContent = fileContent,
                FileName = fileName,
                Path = localFilePath,
                DownloadPath = localFilePath.Replace(".json", "DOWNLOADED.json")
            };
        }

        private static IServiceProvider ConfigureServices()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>(optional: false, reloadOnChange: true)
                .Build();

            return new ServiceCollection()
                .AddSingleton(new BlobServiceClient(configuration.GetConnectionString("StorageAccount")))
                .AddSingleton<FakeEntityGeneratorBase<Review>>(new FakeReviewGenerator())
                .BuildServiceProvider();
        }

        private static void Display(string text)
        {
            AnsiConsole.MarkupLine($"\n[bold yellow2]{Emoji.Known.RightArrow} {text} ...[/]");
            Console.WriteLine();
        }
    }
}
