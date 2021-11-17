using System;
using System.Collections.Generic;
using System.Linq;

namespace TableStorage.CrudApi.Services.Games
{
    public sealed record GameReviewSummaryForUpdate
    {
        public string Platform { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Engine { get; init; } = string.Empty;
        public string Series { get; init; } = string.Empty;
        public string CoverArtLink { get; init; } = string.Empty;
        public string CoverArtThumbnailLink { get; init; } = string.Empty;
        public double? AverageUserRating { get; init; }
        public IReadOnlyList<string> Developers { get; init; } = Array.Empty<string>().ToList();
        public IReadOnlyList<string> Publishers { get; init; } = Array.Empty<string>().ToList();
        public IReadOnlyList<string> Genres { get; init; } = Array.Empty<string>().ToList();
    }
}
