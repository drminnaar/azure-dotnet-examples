using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductManagerFncAppV4.Data;
using ProductManagerFncAppV4.Data.Models;
using ProductManagerFncAppV4.Models;

namespace ProductManagerFncAppV4;

internal sealed class ProductsFunction
{
    private const string ProductImageContainerName = "product-images";
    private readonly ILogger<ProductsFunction> _logger;
    private readonly InventoryDbContext _db;
    private readonly BlobServiceClient _blobService;

    public ProductsFunction(
        ILogger<ProductsFunction> logger,
        InventoryDbContext db,
        BlobServiceClient blobService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _blobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
    }

    [FunctionName(nameof(CreateProduct))]
    public async Task<IActionResult> CreateProduct(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")] HttpRequest request)
    {
        _logger.LogInformation($"[{nameof(CreateProduct)}]: Create product");

        // get product data from request

        using var streamReader = new StreamReader(request.Body);
        var bodyJson = await streamReader.ReadToEndAsync();
        var productForCreate = JsonSerializer.Deserialize<ProductForCreateOrUpdate>(
            bodyJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // upload image to blob storage

        var productId = Guid.NewGuid().ToString().ToLower();
        var productImageUrl = await UploadImage(productId, productForCreate.ImageContent);

        // save product to database

        var product = new ProductEntity
        {
            Id = productId,
            Category = productForCreate.Category,
            Description = productForCreate.Description,
            ImageUrl = productImageUrl,
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
                ImageUrl = product.ImageUrl,
                ImageExtraSmallUrl = product.ImageExtraSmallUrl,
                ImageMediumUrl = product.ImageMediumUrl,
                ImageSmallUrl = product.ImageSmallUrl
            });
    }

    private async Task<string> UploadImage(string productId, string productImageContent)
    {
        var container = _blobService.GetBlobContainerClient(ProductImageContainerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blob = container.GetBlobClient($"{productId.ToLower()}.jpg");
        using var imageStream = new MemoryStream(Convert.FromBase64String(productImageContent));
        await blob.UploadAsync(imageStream, overwrite: true);

        return blob.Uri.ToString();
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

        var container = _blobService.GetBlobContainerClient(ProductImageContainerName);

        if (await container.ExistsAsync())
        {
            var blob = container.GetBlobClient($"{productId.ToLower()}.jpg");
            if (await blob.ExistsAsync())
                await blob.DeleteAsync();
        }

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
            ImageUrl = product.ImageUrl,
            ImageExtraSmallUrl = product.ImageExtraSmallUrl,
            ImageMediumUrl = product.ImageMediumUrl,
            ImageSmallUrl = product.ImageSmallUrl,
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
                ImageUrl = product.ImageUrl,
                ImageExtraSmallUrl = product.ImageExtraSmallUrl,
                ImageMediumUrl = product.ImageMediumUrl,
                ImageSmallUrl = product.ImageSmallUrl,
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
        _logger.LogInformation($"[{nameof(UpdateProduct)}]: Update product having id '{productId}'");

        // get product for update from request

        using var streamReader = new StreamReader(request.Body);
        var bodyJson = await streamReader.ReadToEndAsync();
        var productForUpdate = JsonSerializer.Deserialize<ProductForCreateOrUpdate>(
            bodyJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // find product in db

        var product = await _db.Products.FindAsync(productId);

        if (product is null)
            return new NotFoundResult();

        // update image

        await UploadImage(product.Id, productForUpdate.ImageContent);

        // update product

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
            ImageUrl = product.ImageUrl,
            ImageExtraSmallUrl = product.ImageExtraSmallUrl,
            ImageMediumUrl = product.ImageMediumUrl,
            ImageSmallUrl = product.ImageSmallUrl,
            Price = product.Price,
            Title = product.Title
        });
    }
}
