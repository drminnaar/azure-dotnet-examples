using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ProductManagerFncAppV5.Data;

internal interface IProductImageService
{
    Task DeleteImage(ProductImage productImage);
    Task<ProductImageResizeInfo> ResizeImage(string imageName);
    Task<string> UploadImage(string name, string department, string productImageContent);
}

internal sealed record ProductImageResizeInfo
{
    public string ProductId { get; init; } = string.Empty;
    public string Department { get; init; } = string.Empty;
    public string ImageName { get; init; } = string.Empty;
    public Uri MediumImageUri { get; init; } = null!;
    public Uri SmallImageUri { get; init; } = null!;
    public Uri ExtraSmallImageUri { get; init; } = null!;
}

internal class ProductImageService : IProductImageService
{
    private const string ProductImageContainerName = "product-images";
    private const string ProductImageExtraSmallContainerName = "product-images-xs";
    private const string ProductImageSmallContainerName = "product-images-sm";
    private const string ProductImageMediumContainerName = "product-images-md";

    private readonly BlobServiceClient _blobService;

    public ProductImageService(BlobServiceClient blobService)
    {
        _blobService = blobService
            ?? throw new ArgumentNullException(nameof(blobService));
    }

    public async Task DeleteImage(ProductImage productImage)
    {
        var tasks = new List<Task>
        {
            DeleteProductImage(ProductImageContainerName, productImage),
            DeleteProductImage(ProductImageMediumContainerName, productImage),
            DeleteProductImage(ProductImageSmallContainerName, productImage),
            DeleteProductImage(ProductImageExtraSmallContainerName, productImage)
        };
        await Task.WhenAll(tasks);
    }

    private async Task DeleteProductImage(string containerName, ProductImage productImage)
    {
        var container = _blobService.GetBlobContainerClient(containerName);

        if (await container.ExistsAsync())
        {
            var blob = container.GetBlobClient(productImage.Name);
            if (await blob.ExistsAsync())
                await blob.DeleteAsync();
        }
    }

    public async Task<ProductImageResizeInfo> ResizeImage(string imageName)
    {
        var image = ProductImage.From(imageName);

        var mediumImageUri = await ResizeImage(
            image,
            ImageSize.Medium,
            ProductImageMediumContainerName);

        var smallImageUri = await ResizeImage(
            image,
            ImageSize.Small,
            ProductImageSmallContainerName);

        var extraSmallImageUri = await ResizeImage(
            image,
            ImageSize.ExtraSmall,
            ProductImageExtraSmallContainerName);

        return new ProductImageResizeInfo
        {
            Department = image.Department,
            ImageName = image.Name,
            ProductId = image.ProductId,
            ExtraSmallImageUri = extraSmallImageUri,
            MediumImageUri = mediumImageUri,
            SmallImageUri = smallImageUri
        };
    }

    private async Task<Uri> ResizeImage(
        ProductImage image,
        ImageSize imageSize,
        string containerName)
    {
        var originalImage = await GetOriginalImage(image.Name);
        using var input = Image.Load<Rgba32>(originalImage, out var format);
        using var stream = ResizeImage(input, imageSize, format);

        var container = _blobService.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob);
        var blob = container.GetBlobClient(image.Name);
        var _ = await blob.UploadAsync(stream, overwrite: true);
        return blob.Uri;
    }

    private async Task<Stream> GetOriginalImage(string imageName)
    {
        var container = _blobService.GetBlobContainerClient(ProductImageContainerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob);
        var blob = container.GetBlobClient(imageName);
        return await blob.OpenReadAsync();
    }

    public async Task<string> UploadImage(string productId, string department, string productImageContent)
    {
        var container = _blobService.GetBlobContainerClient(ProductImageContainerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var imageInfo = ProductImage.From(productId, department);
        var blob = container.GetBlobClient(imageInfo.Name);
        using var imageStream = new MemoryStream(Convert.FromBase64String(productImageContent));
        await blob.UploadAsync(imageStream, overwrite: true);

        return blob.Uri.ToString();
    }

    public enum ImageSize { ExtraSmall, Small, Medium }

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
