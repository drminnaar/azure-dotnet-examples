using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using ProductManagerFncAppV1.Models;
using System.Collections.Generic;

namespace ProductManagerFncAppV1;

public static class ProductsFunction
{
    [FunctionName(nameof(CreateProduct))]
    public static async Task<IActionResult> CreateProduct(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")] HttpRequest request,
        ILogger log)
    {
        log.LogInformation($"[{nameof(CreateProduct)}]: Create product");

        using var streamReader = new StreamReader(request.Body);
        var bodyJson = await streamReader.ReadToEndAsync();
        var productForCreate = JsonSerializer.Deserialize<ProductForCreateOrUpdate>(
            bodyJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var product = new ProductForGet
        {
            Id = Guid.NewGuid().ToString().ToUpper(),
            Category = productForCreate.Category,
            Price = productForCreate.Price,
            Description = productForCreate.Description,
            Title = productForCreate.Title,
        };

        return new CreatedAtRouteResult(nameof(GetProduct), new { productId = product.Id }, product);
    }

    [FunctionName(nameof(DeleteProduct))]
    public static async Task<IActionResult> DeleteProduct(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "products/{productId}")] HttpRequest request,
        ILogger log,
        string productId)
    {
        log.LogInformation($"[{nameof(DeleteProduct)}]: Delete product having id '{productId}'");

        await Task.FromResult(string.Empty);

        return new NoContentResult();
    }

    [FunctionName(nameof(GetProduct))]
    public static async Task<IActionResult> GetProduct(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{productId}")] HttpRequest request,
        ILogger log,
        string productId)
    {
        log.LogInformation($"[{nameof(GetProduct)}]: Get product having id '{productId}'");
        var product = await Task.FromResult(new ProductForGet
        {
            Category = "hats",
            Description = "A one of a kind designer cap",
            Price = 10,
            Id = productId,
            Title = "Red Sports Cap"
        });

        return new OkObjectResult(product);
    }

    [FunctionName(nameof(GetProducts))]
    public static async Task<IActionResult> GetProducts(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest request,
        ILogger log)
    {
        log.LogInformation($"[{nameof(GetProducts)}]: Get list of products");

        var products = await Task.FromResult(new List<ProductForGet>
        {
            new ProductForGet
            {
                 Category = "hats",
                 Description = "A one of a kind designer cap",
                 Price = 10,
                 Id = Guid.NewGuid().ToString().ToUpper(),
                 Title = "Red Sports Cap"
            },
            new ProductForGet
            {
                 Category = "shoes",
                 Description = "These shoes are made for walking",
                 Price = 5,
                 Id = Guid.NewGuid().ToString().ToUpper(),
                 Title = "White Sneakers"
            }
        });

        return new OkObjectResult(products);
    }

    [FunctionName(nameof(UpdateProduct))]
    public static async Task<IActionResult> UpdateProduct(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "products/{productId}")] HttpRequest request,
        ILogger log,
        string productId)
    {
        log.LogInformation($"[{nameof(UpdateProduct)}]: Update product for id '{productId}'");

        using var streamReader = new StreamReader(request.Body);
        var bodyJson = await streamReader.ReadToEndAsync();
        var productForUpdate = JsonSerializer.Deserialize<ProductForCreateOrUpdate>(
            bodyJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var product = new ProductForGet
        {
            Id = productId,
            Category = productForUpdate.Category,
            Description = productForUpdate.Description,
            Price = productForUpdate.Price,
            Title = productForUpdate.Title,
        };

        return new OkObjectResult(product);
    }
}
