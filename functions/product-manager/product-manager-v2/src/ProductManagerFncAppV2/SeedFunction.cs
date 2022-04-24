using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ProductManagerFncAppV2.Data;

namespace ProductManagerFncAppV2;

internal sealed class SeedFunction
{
    private readonly ILogger<ProductsFunction> _logger;
    private readonly InventoryDbContext _db;
    private readonly Seeder _seeder;

    public SeedFunction(ILogger<ProductsFunction> logger, InventoryDbContext db, Seeder seeder)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _seeder = seeder ?? throw new ArgumentNullException(nameof(seeder));
    }

    [FunctionName(nameof(Seed))]
    public async Task<IActionResult> Seed(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest request)
    {
        try
        {
            _logger.LogInformation("Seeding database started");
            await _seeder.SeedAsync();
            _logger.LogInformation("Seeding database complete");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "A problem was encountered whilst seeding database");
        }

        return new OkResult();
    }
}
