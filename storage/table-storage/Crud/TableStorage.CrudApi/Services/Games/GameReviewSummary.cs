using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Data.Tables;

namespace TableStorage.CrudApi.Services.Games
{
    public sealed record GameReviewSummary
    {
        public GameReviewSummary()
        {

        }

        public GameReviewSummary(TableEntity summary)
        {
            SummaryId = summary[nameof(GameReviewSummaryTableEntity.RowKey)].ToString() ?? "";
            GameId = summary[nameof(GameReviewSummaryTableEntity.GameId)].ToString() ?? "";
            Platform = summary[nameof(GameReviewSummaryTableEntity.Platform)].ToString() ?? "";
            Title = summary[nameof(GameReviewSummaryTableEntity.Title)].ToString() ?? "";
            Description = summary[nameof(GameReviewSummaryTableEntity.Description)].ToString() ?? "";
            Engine = summary[nameof(GameReviewSummaryTableEntity.Engine)].ToString() ?? "";
            Series = summary[nameof(GameReviewSummaryTableEntity.Series)].ToString() ?? "";
            CoverArtLink = summary[nameof(GameReviewSummaryTableEntity.CoverArtLink)].ToString() ?? "";
            CoverArtThumbnailLink = summary[nameof(GameReviewSummaryTableEntity.CoverArtThumbnailLink)].ToString() ?? "";
            AverageUserRating = Convert.ToDouble(summary[nameof(GameReviewSummaryTableEntity.AverageUserRating)]);
            Developers = summary
                .Keys
                .Where(k => k.StartsWith("Developer"))
                .Select(key => summary[key].ToString()!)
                .ToList();
            Publishers = summary
                .Keys
                .Where(k => k.StartsWith("Publisher"))
                .Select(key => summary[key].ToString()!)
                .ToList();
            Genres = summary
                .Keys
                .Where(k => k.StartsWith("Genre"))
                .Select(key => summary[key].ToString()!)
                .ToList();
        }

        public string SummaryId { get; init; } = string.Empty;
        public string GameId { get; init; } = string.Empty;
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

        public static explicit operator GameReviewSummary(TableEntity summary) => new(summary);
    }
}
