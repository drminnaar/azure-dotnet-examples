using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ProductManagerFncAppV5.Data;
using ProductManagerFncAppV5.Data.Models;
using ProductManagerFncAppV5.Models;

namespace ProductManagerFncAppV5;

internal sealed class SeedFunction
{
    private readonly ILogger _logger;
    private readonly ICosmosDbService<ProductEntity> _db;
    private readonly IProductFaker _productFaker;

    public SeedFunction(
        ILoggerFactory loggerFactory,
        ICosmosDbService<ProductEntity> cosmosDbService,
        IProductFaker productFaker)
    {
        _logger = loggerFactory.CreateLogger<SeedFunction>();
        _db = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _productFaker = productFaker ?? throw new ArgumentNullException(nameof(productFaker));
    }

    [FunctionName(nameof(SeedProducts))]
    public async Task<IActionResult> SeedProducts([HttpTrigger(AuthorizationLevel.Function, "post", Route = "seed")] HttpRequest request)
    {
        try
        {
            // check if db empty
            var productCount = await _db.GetItemCountAsync();
            if (productCount > 1)
            {
                return new ConflictObjectResult(new SeedProductsResponse
                {
                    Message = "Database seed failed due to database already having product data.",
                    ProductCount = productCount
                });
            }

            // get seed info from request
            using var streamReader = new StreamReader(request.Body);
            var bodyJson = await streamReader.ReadToEndAsync();
            var seedProductsRequest = JsonSerializer.Deserialize<SeedProductsRequest>(
                bodyJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // generate fake products
            var products = _productFaker.GenerateRandomProducts(seedProductsRequest.ProductCount);

            // bulk insert fake products
            await _db.BulkInsertAsync(products);

            // return response
            return new OkObjectResult(new SeedProductsResponse
            {
                Message = $"Database seeded successfully",
                ProductCount = products.Count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"[{nameof(SeedFunction)}::{nameof(SeedProducts)}]: {ex.Message}");
            return new InternalServerErrorResult();
        }
    }

    [FunctionName(nameof(PurgeProducts))]
    public async Task<IActionResult> PurgeProducts([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "seed")] HttpRequest request)
    {
        try
        {
            // get number of products before purge
            var count = await _db.GetItemCountAsync();

            // delete all products from database
            await _db.PurgeAsync();

            // return response
            return new OkObjectResult(new PurgeProductsResponse
            {
                Message = $"Database purged successfully",
                NumberOfProductsPurged = count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"[{nameof(SeedFunction)}::{nameof(PurgeProducts)}]: {ex.Message}");
            return new InternalServerErrorResult();
        }
    }
}
