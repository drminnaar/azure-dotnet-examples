using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ProductManagerFncAppV5.Data;

namespace ProductManagerFncAppV5;

internal class ProductImageResizeFunction
{
    private readonly ILogger<ProductImageResizeFunction> _logger;
    private readonly IProductImageService _productImageService;
    private readonly IProductService _productService;

    public ProductImageResizeFunction(
        ILogger<ProductImageResizeFunction> logger,
        IProductImageService productImageService,
        IProductService productService)
    {
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        _productImageService = productImageService
            ?? throw new ArgumentNullException(nameof(productImageService));

        _productService = productService
            ?? throw new ArgumentNullException(nameof(productService));
    }

    [FunctionName(nameof(ResizeProductImage))]
    public async Task ResizeProductImage(
        [BlobTrigger("product-images/{name}", Connection = "StorageAccount")] Stream image,
        string name)
    {
        try
        {
            var imageInfo = await _productImageService.ResizeImage(name);
            await _productService.UpdateProductImageInfo(imageInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError($"[{nameof(ProductImageResizeFunction)}::{nameof(ResizeProductImage)}]: {ex.Message}");
        }
    }
}
