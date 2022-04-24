using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ProductManagerFncAppV5.Data.Models;

internal sealed class ProductEntity : ICosmosDbItem
{
    internal const string EntityType = "product";

    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("pk")]
    public string Pk { get; set; } = string.Empty;

    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [JsonProperty("department")]
    public string Department { get; set; } = string.Empty;

    [JsonProperty("imageUrl")]
    public string ImageUrl { get; set; } = string.Empty;

    [JsonProperty("image-md-url")]
    public string ImageMediumUrl { get; set; } = string.Empty;

    [JsonProperty("image-sm-url")]
    public string ImageSmallUrl { get; set; } = string.Empty;

    [JsonProperty("image-xs-url")]
    public string ImageExtraSmallUrl { get; set; } = string.Empty;

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("categories")]
    public List<string> Categories { get; set; } = Array.Empty<string>().ToList();

    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}
