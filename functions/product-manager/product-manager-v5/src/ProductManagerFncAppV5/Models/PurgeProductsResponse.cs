namespace ProductManagerFncAppV5.Models
{
    internal sealed record PurgeProductsResponse
    {
        public string Message { get; init; } = string.Empty;
        public int NumberOfProductsPurged { get; init; }
    }
}
