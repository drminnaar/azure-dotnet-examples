using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagerFncAppV3.Data;

[assembly: FunctionsStartup(typeof(ProductManagerFncAppV3.Startup))]

namespace ProductManagerFncAppV3;

internal sealed class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var isDevelopment = builder
            .GetContext()
            .EnvironmentName
            .ToLower() == "development";

        var storageAccountConnection = builder
            .GetContext()
            .Configuration
            .GetConnectionStringOrSetting("StorageAccount");

        builder
            .Services
            .AddDbContextPool<InventoryDbContext>(options =>
            {
                options.UseInMemoryDatabase("inventory");
                options.EnableDetailedErrors(isDevelopment);
                options.EnableSensitiveDataLogging(isDevelopment);
            })
            .AddSingleton(new BlobServiceClient(storageAccountConnection))
            .AddScoped<Seeder>()
            .AddLogging();
    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        base.ConfigureAppConfiguration(builder);
        builder
            .ConfigurationBuilder
            .AddUserSecrets<Startup>(optional: false, reloadOnChange: true);
    }
}
