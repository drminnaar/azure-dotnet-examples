using System.Collections.Generic;

namespace TableStorage.CrudApi.Services.Games
{
    public sealed class GameReviewSummaryFilterBuilder
    {
        public string Filter => string.Join(" and ", _filters);

        private readonly IList<string> _filters = new List<string>
        {
            $"{nameof(GameReviewSummaryTableEntity.Type)} eq '{nameof(GameReviewSummary)}'"
        };

        public GameReviewSummaryFilterBuilder WhereMaxAverageUserRating(double? rating)
        {
            if (rating.HasValue)
                _filters.Add($"{nameof(GameReviewSummaryTableEntity.AverageUserRating)} le {rating}");

            return this;
        }

        public GameReviewSummaryFilterBuilder WhereMinAverageUserRating(double? rating)
        {
            if (rating.HasValue)
                _filters.Add($"{nameof(GameReviewSummaryTableEntity.AverageUserRating)} ge {rating}");

            return this;
        }

        public GameReviewSummaryFilterBuilder WherePlatformEquals(string? platform)
        {
            if (!string.IsNullOrWhiteSpace(platform))
                _filters.Add($"{nameof(GameReviewSummaryTableEntity.PartitionKey)} eq '{platform.ToLowerInvariant()}'");

            return this;
        }

        public GameReviewSummaryFilterBuilder WhereTitleEquals(string? title)
        {
            if (!string.IsNullOrWhiteSpace(title))
                _filters.Add($"{nameof(GameReviewSummaryTableEntity.RowKey)} eq '{title.ToLowerInvariant()}'");

            return this;
        }
    }
}
