using System;

namespace CosmosDb.CrudApi.Services.Games;

public sealed record GameQueryInput
{
    public GameQueryInput(string platform)
    {
        if (string.IsNullOrWhiteSpace(platform))
        {
            throw new ArgumentException($"'{nameof(platform)}' cannot be null or whitespace.", nameof(platform));
        }
        Platform = platform;
    }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string Platform { get; init; } = string.Empty;
}
