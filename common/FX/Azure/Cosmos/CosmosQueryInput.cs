namespace Azure.Cosmos;

public record CosmosQueryInput
{
    public string PartitionKey { get; init; } = string.Empty;
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}
