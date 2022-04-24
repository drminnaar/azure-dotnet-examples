using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductManagerFncAppV2.Data;

[assembly: FunctionsStartup(typeof(ProductManagerFncAppV2.Startup))]

namespace ProductManagerFncAppV2;

internal sealed class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var isDevelopment = builder
            .GetContext()
            .EnvironmentName
            .ToLower() == "development";

        builder
            .Services
            .AddDbContextPool<InventoryDbContext>(options =>
            {
                options.UseInMemoryDatabase("inventory");
                options.EnableDetailedErrors(isDevelopment);
                options.EnableSensitiveDataLogging(isDevelopment);
            })
            .AddScoped<Seeder>()
            .AddLogging();
    }
}
