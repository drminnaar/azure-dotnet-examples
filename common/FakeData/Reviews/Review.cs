using System;
using System.Text.Json.Serialization;

namespace FakeData.Reviews
{
    public sealed record Review : FakeEntityBase<Review>
    {
        public Review() : base()
        {
        }

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

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
