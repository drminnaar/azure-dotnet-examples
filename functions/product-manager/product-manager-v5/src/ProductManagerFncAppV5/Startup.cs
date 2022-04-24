using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagerFncAppV5.Data;
using ProductManagerFncAppV5.Data.Models;

[assembly: FunctionsStartup(typeof(ProductManagerFncAppV5.Startup))]

namespace ProductManagerFncAppV5;

internal sealed class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var storageAccountConnection = builder
            .GetContext()
            .Configuration
            .GetConnectionStringOrSetting("StorageAccount");

        builder
            .Services
            .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
            .AddLogging()
            .AddSingleton<IEntityIdProvider, HashEntityIdProvider>()
            .AddSingleton<IProductFaker, ProductFaker>()
            .AddSingleton(new BlobServiceClient(storageAccountConnection))
            .AddCosmosDb(builder.GetContext().Configuration)
            .AddSingleton<IProductImageService, ProductImageService>()
            .AddSingleton<IProductService, ProductService>();
    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        base.ConfigureAppConfiguration(builder);
        builder
            .ConfigurationBuilder
            .AddUserSecrets<Startup>(optional: false, reloadOnChange: true);
    }
}
