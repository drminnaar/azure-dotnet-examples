using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using ProductManagerFncAppV5.Data.Models;
using ProductManagerFncAppV5.Models;

namespace ProductManagerFncAppV5.Data;

internal interface IProductService
{
    Task<ProductForGet> CreateProduct(ProductForCreateOrUpdate product);
    Task DeleteProduct(string productId, string department);
    Task<ProductForGet> GetProduct(string productId, string department);
    Task<IPagedCollection<ProductForGet>> GetProducts(int page, int pageSize, string department = "");
    Task<ProductForGet> UpdateProduct(string productId, string department, ProductForCreateOrUpdate productForUpdate);
    Task<ProductForGet> UpdateProductImageInfo(ProductImageResizeInfo imageInfo);
}

internal sealed class ProductService : IProductService
{
    private readonly ICosmosDbService<ProductEntity> _cosmosDbService;
    private readonly IProductImageService _imageService;
    private readonly IEntityIdProvider _entityIdProvider;

    public ProductService(
        ICosmosDbService<ProductEntity> cosmosDbService,
        IProductImageService imageService,
        IEntityIdProvider entityIdProvider)
    {
        _cosmosDbService = cosmosDbService
            ?? throw new ArgumentNullException(nameof(cosmosDbService));

        _imageService = imageService
            ?? throw new ArgumentNullException(nameof(imageService));

        _entityIdProvider = entityIdProvider
            ?? throw new ArgumentNullException(nameof(entityIdProvider));
    }

    public async Task<ProductForGet> CreateProduct(ProductForCreateOrUpdate product)
    {
        // upload image to blob storage
        var productId = _entityIdProvider.GetEntityId();
        var productImageUrl = await _imageService.UploadImage(
            productId,
            product.Department,
            product.ImageContent);

        // map to product entity
        var productEntity = new ProductEntity
        {
            Categories = product.Categories.Select(category => category.ToLower()).ToList(),
            CreatedAt = DateTime.UtcNow,
            Description = product.Description,
            Department = product.Department,
            Id = productId,
            ImageUrl = productImageUrl,
            Pk = $"{product.Department.ToLower()}",
            Price = product.Price,
            Title = product.Title,
            Type = ProductEntity.EntityType,
            UpdatedAt = DateTime.UtcNow,
        };

        // save product entity to cosmosdb
        await _cosmosDbService.CreateItemAsync(
            productEntity,
            new PartitionKey(productEntity.Pk));

        // map from product entity to product DTO
        return new ProductForGet
        {
            Categories = productEntity.Categories,
            Department = productEntity.Department,
            Description = productEntity.Description,
            Id = productEntity.Id,
            Price = productEntity.Price,
            Title = productEntity.Title,
            ImageUrl = productEntity.ImageUrl,
            ImageExtraSmallUrl = productEntity.ImageExtraSmallUrl,
            ImageMediumUrl = productEntity.ImageMediumUrl,
            ImageSmallUrl = productEntity.ImageSmallUrl
        };
    }

    public async Task DeleteProduct(string productId, string department)
    {
        await _cosmosDbService.DeleteItemAsync(
            productId,
            new PartitionKey(department.ToLower()));

        await _imageService.DeleteImage(ProductImage.From(productId, department));
    }

    public async Task<IPagedCollection<ProductForGet>> GetProducts(int page, int pageSize, string department = "")
    {
        var productCollection = await _cosmosDbService.GetItemsAsync(page, pageSize, department?.ToLower());

        var products = productCollection.Select(product => new ProductForGet
        {
            Categories = product.Categories,
            Department = product.Department,
            Description = product.Description,
            Id = product.Id,
            Price = product.Price,
            Title = product.Title,
            ImageUrl = product.ImageUrl,
            ImageExtraSmallUrl = product.ImageExtraSmallUrl,
            ImageMediumUrl = product.ImageMediumUrl,
            ImageSmallUrl = product.ImageSmallUrl
        })
        .ToList();

        return new PagedCollection<ProductForGet>(
            products,
            productCollection.ItemCount,
            productCollection.CurrentPageNumber,
            productCollection.PageSize);
    }

    public async Task<ProductForGet> GetProduct(string productId, string department)
    {
        var product = await _cosmosDbService.GetItemAsync(
            productId,
            new PartitionKey(department.ToLower()));

        return product is null ? default : new ProductForGet
        {
            Categories = product.Categories,
            Department = product.Department,
            Description = product.Description,
            Id = product.Id,
            Price = product.Price,
            Title = product.Title,
            ImageUrl = product.ImageUrl,
            ImageExtraSmallUrl = product.ImageExtraSmallUrl,
            ImageMediumUrl = product.ImageMediumUrl,
            ImageSmallUrl = product.ImageSmallUrl
        };
    }

    public async Task<ProductForGet> UpdateProduct(
        string productId,
        string department,
        ProductForCreateOrUpdate productForUpdate)
    {
        var product = await _cosmosDbService.GetItemAsync(
            productId,
            new PartitionKey(department.ToLower()));

        if (product is null)
            return default;

        product.Description = productForUpdate.Description;
        product.Categories = productForUpdate.Categories.Select(category => category.ToLower()).ToList();
        product.Price = productForUpdate.Price;
        product.Title = productForUpdate.Title;
        product.UpdatedAt = DateTime.UtcNow;

        await _cosmosDbService.UpdateItemAsync(
            product,
            new PartitionKey(department.ToLower()));

        await _imageService.UploadImage(product.Id, product.Department, productForUpdate.ImageContent);

        return new ProductForGet
        {
            Categories = product.Categories,
            Department = product.Department,
            Description = product.Description,
            Id = product.Id,
            Price = product.Price,
            Title = product.Title,
            ImageUrl = product.ImageUrl,
            ImageExtraSmallUrl = product.ImageExtraSmallUrl,
            ImageMediumUrl = product.ImageMediumUrl,
            ImageSmallUrl = product.ImageSmallUrl
        };
    }

    public async Task<ProductForGet> UpdateProductImageInfo(ProductImageResizeInfo imageInfo)
    {
        var product = await _cosmosDbService.GetItemAsync(
            imageInfo.ProductId,
            new PartitionKey(imageInfo.Department.ToLower()));

        if (product is null) return default;

        product.ImageMediumUrl = imageInfo.MediumImageUri.ToString();
        product.ImageExtraSmallUrl = imageInfo.ExtraSmallImageUri.ToString();
        product.ImageSmallUrl = imageInfo.SmallImageUri.ToString();

        await _cosmosDbService.UpdateItemAsync(
            product,
            new PartitionKey(imageInfo.Department.ToLower()));

        return new ProductForGet
        {
            Categories = product.Categories,
            Department = product.Department,
            Description = product.Description,
            Id = product.Id,
            Price = product.Price,
            Title = product.Title,
            ImageUrl = product.ImageUrl,
            ImageExtraSmallUrl = product.ImageExtraSmallUrl,
            ImageMediumUrl = product.ImageMediumUrl,
            ImageSmallUrl = product.ImageSmallUrl
        };
    }
}
