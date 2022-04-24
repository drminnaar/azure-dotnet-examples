using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductManagerFncAppV2.Data;
using ProductManagerFncAppV2.Data.Models;
using ProductManagerFncAppV2.Models;

namespace ProductManagerFncAppV2;

internal sealed class ProductsFunction
{
    private readonly ILogger<ProductsFunction> _logger;
    private readonly InventoryDbContext _db;

    public ProductsFunction(ILogger<ProductsFunction> logger, InventoryDbContext db)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    [FunctionName(nameof(CreateProduct))]
    public async Task<IActionResult> CreateProduct(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")] HttpRequest request)
    {
        _logger.LogInformation($"[{nameof(CreateProduct)}]: Create product");

        using var streamReader = new StreamReader(request.Body);
        var bodyJson = await streamReader.ReadToEndAsync();
        var productForCreate = JsonSerializer.Deserialize<ProductForCreateOrUpdate>(
            bodyJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var product = new ProductEntity
        {
            Id = Guid.NewGuid().ToString().ToUpper(),
            Category = productForCreate.Category,
            Description = productForCreate.Description,
            Price = productForCreate.Price,
            Title = productForCreate.Title,
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return new CreatedAtRouteResult(
            routeName: nameof(GetProduct),
            routeValues: new { productId = product.Id },
            value: new ProductForGet
            {
                Category = product.Category,
                Description = product.Description,
                Id = product.Id,
                Price = product.Price,
                Title = product.Title,
            });
    }

    [FunctionName(nameof(DeleteProduct))]
    public async Task<IActionResult> DeleteProduct(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "products/{productId}")] HttpRequest request,
        string productId)
    {
        _logger.LogInformation($"[{nameof(DeleteProduct)}]: Delete product having id '{productId}'");

        var product = await _db.Products.FindAsync(productId);

        if (product is null)
            return new NotFoundResult();

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();

        return new NoContentResult();
    }

    [FunctionName(nameof(GetProduct))]
    public async Task<IActionResult> GetProduct(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{productId}")] HttpRequest request,
        string productId)
    {
        _logger.LogInformation($"[{nameof(GetProduct)}]: Get product having id '{productId}'");

        var product = await _db.Products.FindAsync(productId);

        return product is null ? new NotFoundResult() : new OkObjectResult(new ProductForGet
        {
            Category = product.Category,
            Description = product.Description,
            Id = product.Id,
            Price = product.Price,
            Title = product.Title,
        });
    }

    [FunctionName(nameof(GetProducts))]
    public async Task<IActionResult> GetProducts(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest request)
    {
        _logger.LogInformation($"[{nameof(GetProducts)}]: Get list of products");

        var products = await _db
            .Products
            .AsNoTracking()
            .ToListAsync();

        var productsForGet = products
            .Select(product => new ProductForGet
            {
                Category = product.Category,
                Description = product.Description,
                Id = product.Id,
                Price = product.Price,
                Title = product.Title,
            })
            .ToList();

        return new OkObjectResult(productsForGet);
    }

    [FunctionName(nameof(UpdateProduct))]
    public async Task<IActionResult> UpdateProduct(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "products/{productId}")] HttpRequest request,
        string productId)
    {
        _logger.LogInformation($"[{nameof(UpdateProduct)}]: Update product for id '{productId}'");

        using var streamReader = new StreamReader(request.Body);
        var bodyJson = await streamReader.ReadToEndAsync();
        var productForUpdate = JsonSerializer.Deserialize<ProductForCreateOrUpdate>(
            bodyJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var product = await _db.Products.FindAsync(productId);

        if (product is null)
            return new NotFoundResult();

        product.Category = productForUpdate.Category;
        product.Description = productForUpdate.Description;
        product.Price = productForUpdate.Price;
        product.Title = productForUpdate.Title;

        await _db.SaveChangesAsync();

        return new OkObjectResult(new ProductForGet
        {
            Category = product.Category,
            Description = product.Description,
            Id = product.Id,
            Price = product.Price,
            Title = product.Title
        });
    }
}
