using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace CosmosDb.CrudApi.Services.Games;

public sealed record GameEntity
{
    public const string EntityType = "Game";

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("pk")]
    public string Pk { get; set; } = string.Empty;

    [JsonPropertyName("gameId")]
    public string GameId { get; init; } = string.Empty;

    [JsonPropertyName("platform")]
    public string Platform { get; init; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; init; } = string.Empty;

    [JsonPropertyName("engine")]
    public string Engine { get; init; } = string.Empty;

    [JsonPropertyName("series")]
    public string Series { get; init; } = string.Empty;

    [JsonPropertyName("coverArtLink")]
    public string CoverArtLink { get; init; } = string.Empty;

    [JsonPropertyName("coverArtThumbnailLink")]
    public string CoverArtThumbnailLink { get; init; } = string.Empty;

    [JsonPropertyName("developers")]
    public List<string> Developers { get; init; } = Array.Empty<string>().ToList();

    [JsonPropertyName("publishers")]
    public List<string> Publishers { get; init; } = Array.Empty<string>().ToList();

    [JsonPropertyName("genres")]
    public List<string> Genres { get; init; } = Array.Empty<string>().ToList();

    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTimeOffset UpdatedAt { get; set; }
}
