namespace ProductManagerFncAppV4.Models;

internal sealed record ProductForGet
{
    public string Id { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Title { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public string ImageMediumUrl { get; init; } = string.Empty;
    public string ImageSmallUrl { get; init; } = string.Empty;
    public string ImageExtraSmallUrl { get; init; } = string.Empty;
}
