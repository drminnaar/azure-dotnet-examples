using System;
using System.Collections.Generic;
using System.Linq;

namespace CosmosDb.CrudApi.Services.Games;

public sealed record GameForQuery
{
    public string Id { get; internal init; } = string.Empty;

    public string GameId { get; internal init; } = string.Empty;

    public string Platform { get; internal init; } = string.Empty;

    public string Title { get; internal init; } = string.Empty;

    public string Description { get; internal init; } = string.Empty;

    public string Engine { get; internal init; } = string.Empty;

    public string Series { get; internal init; } = string.Empty;

    public string CoverArtLink { get; internal init; } = string.Empty;

    public string CoverArtThumbnailLink { get; internal init; } = string.Empty;

    public IReadOnlyList<string> Developers { get; internal init; } = Array.Empty<string>().ToList();

    public IReadOnlyList<string> Publishers { get; internal init; } = Array.Empty<string>().ToList();

    public IReadOnlyList<string> Genres { get; internal init; } = Array.Empty<string>().ToList();

    public DateTimeOffset CreatedAt { get; internal init; }

    public DateTimeOffset UpdatedAt { get; internal init; }
}
