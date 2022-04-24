namespace ProductManagerFncAppV5.Models
{
    internal sealed record SeedProductsResponse
    {
        public string Message { get; init; } = string.Empty;
        public int ProductCount { get; init; }
    }
}
