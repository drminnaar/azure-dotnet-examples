using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FakeData.Reviews
{
    public sealed record ReviewForCosmos : FakeEntityBase<ReviewForCosmos>
    {
        public ReviewForCosmos() : base()
        {
        }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("pk")]
        public string Pk { get; set; } = string.Empty;

        [JsonPropertyName("reviewId")]
        public string ReviewId { get; init; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; init; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; init; } = string.Empty;

        [JsonPropertyName("rating")]
        public int Rating { get; init; }

        [JsonPropertyName("reviewedBy")]
        public string ReviewedBy { get; init; } = string.Empty;

        [JsonPropertyName("reviewedAt")]
        public DateTimeOffset ReviewedAt { get; set; }

        public override int GetConsistentHashCode() => Type.GetConsistentHashCode();

        public override string ToString()
        {
            return $"{Type.ToUpperInvariant()}::{{{nameof(Title)}: {Title}, {nameof(Rating)}: {Rating}}}";
        }
    }
}
