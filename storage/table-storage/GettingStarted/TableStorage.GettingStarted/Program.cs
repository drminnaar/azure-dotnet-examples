using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Data.Tables;
using FakeData;
using FakeData.Weather;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace TableStorage.GettingStarted
{
    internal sealed class Program
    {
        internal static async Task Main()
        {
            DisplayHeader();

            try
            {
                await ConfigureServices()
                    .GetRequiredService<TableStorageTest>()
                    .Run();
            }
            catch (Exception error)
            {
                LogError(error);
            }

            Console.ReadLine();
        }

        private static IServiceProvider ConfigureServices()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>(optional: false, reloadOnChange: true)
                .Build();

            return new ServiceCollection()
                .AddSingleton(new TableServiceClient(configuration.GetConnectionString("WeatherData")))
                .AddSingleton(new TableClient(configuration.GetConnectionString("WeatherData"), "Forecasts"))
                .AddSingleton<FakeEntityGeneratorBase<Forecast>>(new FakeForecastGenerator())
                .AddSingleton<TableStorageTest>()
                .BuildServiceProvider();
        }

        private static void DisplayHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Text($"AZURE TABLE STORAGE", new Style(foreground: Color.LightSalmon1)).Centered());
            AnsiConsole.Write(new Text($"GETTING STARTED", new Style(foreground: Color.LightSalmon1)).Centered());
            Console.WriteLine();
        }

        private static void LogError(Exception error)
        {
            AnsiConsole.MarkupLine($"[bold red1]ERROR::{error.Message}[/]");
        }
    }
}
