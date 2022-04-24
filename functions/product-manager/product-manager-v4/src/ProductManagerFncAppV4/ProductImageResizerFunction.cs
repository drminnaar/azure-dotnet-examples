using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ProductManagerFncAppV4.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ProductManagerFncAppV4;

internal sealed class ProductImageResizerFunction
{
    private readonly ILogger<ProductImageResizerFunction> _logger;
    private readonly InventoryDbContext _db;

    public ProductImageResizerFunction(ILogger<ProductImageResizerFunction> logger, InventoryDbContext db)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    [FunctionName(nameof(ResizeProductImage))]
    public async Task ResizeProductImage(
        [BlobTrigger("product-images/{name}", Connection = "StorageAccount")] Stream image,
        [Blob("product-images-md/{name}", FileAccess.Write)] BlobClient mediumBlob,
        [Blob("product-images-sm/{name}", FileAccess.Write)] BlobClient smallBlob,
        [Blob("product-images-xs/{name}", FileAccess.Write)] BlobClient extraSmallBlob,
        string name)
    {
        _logger.LogInformation($"[{nameof(ProductImageResizerFunction)}::{nameof(ResizeProductImage)}]: Blob trigger function Processed blob\n Name:{name} \n Size: {image.Length} Bytes");

        IImageFormat format;

        using (var input = Image.Load<Rgba32>(image, out format))
        {
            using var stream = ResizeImage(input, ImageSize.Medium, format);
            await mediumBlob.UploadAsync(stream, overwrite: true);
        }

        image.Position = 0;
        using (var input = Image.Load<Rgba32>(image, out format))
        {
            using var stream = ResizeImage(input, ImageSize.Small, format);
            await smallBlob.UploadAsync(stream, overwrite: true);
        }

        image.Position = 0;
        using (var input = Image.Load<Rgba32>(image, out format))
        {
            using var stream = ResizeImage(input, ImageSize.ExtraSmall, format);
            await extraSmallBlob.UploadAsync(stream, overwrite: true);
        }

        var productId = Path.GetFileNameWithoutExtension(name).ToLower();
        var product = await _db
            .Products
            .FindAsync(productId);

        if (product is not null)
        {
            product.ImageMediumUrl = mediumBlob.Uri.ToString();
            product.ImageSmallUrl = smallBlob.Uri.ToString();
            product.ImageExtraSmallUrl = extraSmallBlob.Uri.ToString();
            await _db.SaveChangesAsync();
        }
    }

    private enum ImageSize { ExtraSmall, Small, Medium }

    private static Stream ResizeImage(Image<Rgba32> input, ImageSize size, IImageFormat format)
    {
        var output = new MemoryStream();

        var (width, height) = imageDimensionsTable[size];
        input.Mutate(x => x.Resize(width, height));
        input.Save(output, format);
        output.Position = 0;

        return output;
    }

    private static readonly Dictionary<ImageSize, (int Width, int Height)> imageDimensionsTable = new()
    {
        { ImageSize.ExtraSmall, (50, 50) },
        { ImageSize.Small, (150, 150) },
        { ImageSize.Medium, (250, 250) }
    };
}
