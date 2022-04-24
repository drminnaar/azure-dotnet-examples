namespace ProductManagerFncAppV4.Data.Models;

internal sealed class ProductEntity
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string ImageMediumUrl { get; set; } = string.Empty;
    public string ImageSmallUrl { get; set; } = string.Empty;
    public string ImageExtraSmallUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
