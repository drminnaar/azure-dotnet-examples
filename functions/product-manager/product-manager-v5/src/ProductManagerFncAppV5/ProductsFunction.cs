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
using ProductManagerFncAppV5.Models;

namespace ProductManagerFncAppV5;

internal sealed class ProductsFunction
{
    private readonly ILogger<ProductsFunction> _logger;
    private readonly IProductService _productService;

    public ProductsFunction(ILogger<ProductsFunction> logger, IProductService productService)
    {
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        _productService = productService
            ?? throw new ArgumentNullException(nameof(productService));
    }

    [FunctionName(nameof(CreateProduct))]
    public async Task<IActionResult> CreateProduct(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")] HttpRequest request)
    {
        try
        {
            // get product data from request
            using var streamReader = new StreamReader(request.Body);
            var bodyJson = await streamReader.ReadToEndAsync();
            var productForCreate = JsonSerializer.Deserialize<ProductForCreateOrUpdate>(
                bodyJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // create product
            var product = await _productService.CreateProduct(productForCreate);

            // return response
            return new CreatedAtRouteResult(
                routeName: nameof(GetProduct),
                routeValues: new { department = product.Department, productId = product.Id },
                value: product);
        }
        catch (Exception ex)
        {
            _logger.LogError($"[{nameof(ProductsFunction)}::{nameof(CreateProduct)}]: {ex.Message}");
            return new InternalServerErrorResult();
        }
    }

    [FunctionName(nameof(DeleteProduct))]
    public async Task<IActionResult> DeleteProduct(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "products/{department}/{productId}")] HttpRequest request,
        string department,
        string productId)
    {
        try
        {
            var product = await _productService.GetProduct(productId, department);

            if (product is null)
                return new NotFoundResult();

            await _productService.DeleteProduct(productId, department);

            return new NoContentResult();
        }
        catch (Exception ex)
        {
            _logger.LogError($"[{nameof(ProductsFunction)}::{nameof(DeleteProduct)}]: {ex.Message}");
            return new InternalServerErrorResult();
        }
    }

    [FunctionName(nameof(UpdateProduct))]
    public async Task<IActionResult> UpdateProduct(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "products/{department}/{productId}")] HttpRequest request,
        string department,
        string productId)
    {
        try
        {
            // get product data from request
            using var streamReader = new StreamReader(request.Body);
            var bodyJson = await streamReader.ReadToEndAsync();
            var productForUpdate = JsonSerializer.Deserialize<ProductForCreateOrUpdate>(
                bodyJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // check if product exists
            if (await _productService.GetProduct(productId, department) is null)
                return new NotFoundResult();

            // update product
            var productFromUpdate = await _productService.UpdateProduct(productId, department, productForUpdate);

            // return response
            return new OkObjectResult(productFromUpdate);
        }
        catch (Exception ex)
        {
            _logger.LogError($"[{nameof(ProductsFunction)}::{nameof(UpdateProduct)}]: {ex.Message}");
            return new InternalServerErrorResult();
        }
    }

    [FunctionName(nameof(GetProduct))]
    public async Task<IActionResult> GetProduct(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{department}/{productId}")] HttpRequest request,
        string department,
        string productId)
    {
        try
        {
            // find product
            var product = await _productService.GetProduct(productId, department);

            // return response
            return product is null ? new NotFoundResult() : new OkObjectResult(product);
        }
        catch (Exception ex)
        {
            _logger.LogError($"[{nameof(ProductsFunction)}::{nameof(GetProduct)}]: {ex.Message}");
            return new InternalServerErrorResult();
        }
    }

    [FunctionName(nameof(GetPagedProducts))]
    public async Task<IActionResult> GetPagedProducts(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest request)
    {
        try
        {
            var pageNumber = int.TryParse(request.Query["page"], out var page) ? page : 1;
            var pageSize = int.TryParse(request.Query["size"], out var size) ? size : 20;

            // get paged collection of products
            var products = await _productService.GetProducts(pageNumber, pageSize);

            // return response
            return new OkObjectResult(new
            {
                productCount = products.ItemCount,
                pageCount = products.PageCount,
                pageSize = products.PageSize,
                currentPageNumber = products.CurrentPageNumber,
                nextPageNumber = products.NextPageNumber,
                previousPageNumber = products.PreviousPageNumber,
                hasNextPage = products.HasNext,
                hasPreviousPage = products.HasPrevious,
                products
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"[{nameof(ProductsFunction)}::{nameof(GetProduct)}]: {ex.Message}");
            return new InternalServerErrorResult();
        }
    }

    [FunctionName(nameof(GetPagedProductsByDepartment))]
    public async Task<IActionResult> GetPagedProductsByDepartment(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{department}")] HttpRequest request,
        string department)
    {
        try
        {
            var pageNumber = int.TryParse(request.Query["page"], out var page) ? page : 1;
            var pageSize = int.TryParse(request.Query["size"], out var size) ? size : 20;

            // get paged collection of products
            var products = await _productService.GetProducts(pageNumber, pageSize, department);

            // return response
            return new OkObjectResult(new
            {
                productCount = products.ItemCount,
                pageCount = products.PageCount,
                pageSize = products.PageSize,
                currentPageNumber = products.CurrentPageNumber,
                nextPageNumber = products.NextPageNumber,
                previousPageNumber = products.PreviousPageNumber,
                hasNextPage = products.HasNext,
                hasPreviousPage = products.HasPrevious,
                products
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"[{nameof(ProductsFunction)}::{nameof(GetProduct)}]: {ex.Message}");
            return new InternalServerErrorResult();
        }
    }
}
