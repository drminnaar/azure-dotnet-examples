using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Azure;
using Azure.Data.Tables;

namespace TableStorage.CrudApi.Services.Games
{
    public sealed class GameReviewSummaryTableEntity : ITableEntity
    {
        public const string TableEntityType = "GameReviewSummary";

        private readonly IDictionary<string, object> _properties = new Dictionary<string, object>();
        private const string DeveloperPropertyPrefix = "Developer";
        private const string PublisherPropertyPrefix = "Publisher";
        private const string GenrePropertyPrefix = "Genre";

        public GameReviewSummaryTableEntity()
        {
        }

        public GameReviewSummaryTableEntity(GameReviewSummaryForCreate game)
        {
            AverageUserRating = game.AverageUserRating;
            CoverArtLink = game.CoverArtLink;
            CoverArtThumbnailLink = game.CoverArtThumbnailLink;
            Description = game.Description;
            Engine = game.Engine;
            GameId = game.GameId;
            PartitionKey = game.Platform.ToLowerInvariant();
            Platform = game.Platform;
            RowKey = game.Title.ToLowerInvariant();
            Series = game.Series;
            Title = game.Title;
            Type = TableEntityType;
            AddGenres(game.Genres);
            AddPublishers(game.Publishers);
            AddDevelopers(game.Developers);
        }

        public GameReviewSummaryTableEntity(TableEntity summary)
        {
            PartitionKey = summary.PartitionKey;
            RowKey = summary.RowKey;
            ETag = summary.ETag;
            Timestamp = summary.Timestamp;
            GameId = summary[nameof(GameId)].ToString() ?? "";
            Platform = summary[nameof(Platform)].ToString() ?? "";
            Title = summary[nameof(Title)].ToString() ?? "";
            Description = summary[nameof(Description)].ToString() ?? "";
            Engine = summary[nameof(Engine)].ToString() ?? "";
            Series = summary[nameof(Series)].ToString() ?? "";
            CoverArtLink = summary[nameof(CoverArtLink)].ToString() ?? "";
            CoverArtThumbnailLink = summary[nameof(CoverArtThumbnailLink)].ToString() ?? "";
            AverageUserRating = Convert.ToDouble(summary[nameof(AverageUserRating)]);
            Type = TableEntityType;

            var developers = summary
                .Where(x => x.Key.StartsWith(DeveloperPropertyPrefix)).Select(kvp => kvp.Value.ToString() ?? "")
                .ToList();
            AddDevelopers(developers);

            var publishers = summary
                .Where(x => x.Key.StartsWith(PublisherPropertyPrefix)).Select(kvp => kvp.Value.ToString() ?? "")
                .ToList();
            AddPublishers(publishers);

            var genres = summary
                .Where(x => x.Key.StartsWith(GenrePropertyPrefix)).Select(kvp => kvp.Value.ToString() ?? "")
                .ToList();
            AddGenres(genres);
        }

        public GameReviewSummaryTableEntity(GameReviewSummaryTableEntity summary)
        {
            PartitionKey = summary.PartitionKey;
            RowKey = summary.RowKey;
            ETag = summary.ETag;
            Timestamp = summary.Timestamp;
            GameId = summary.GameId;
            Platform = summary.Platform;
            Title = summary.Title;
            Description = summary.Description;
            Engine = summary.Engine;
            Series = summary.Series;
            CoverArtLink = summary.CoverArtLink;
            CoverArtThumbnailLink = summary.CoverArtThumbnailLink;
            AverageUserRating = summary.AverageUserRating;
            Type = summary.Type;
        }

        public string PartitionKey { get; set; } = string.Empty;
        public string RowKey { get; set; } = string.Empty;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string Platform { get; init; } = string.Empty;
        public string GameId { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Engine { get; init; } = string.Empty;
        public string Series { get; init; } = string.Empty;
        public string CoverArtLink { get; init; } = string.Empty;
        public string CoverArtThumbnailLink { get; init; } = string.Empty;
        public double? AverageUserRating { get; init; }
        public string Type { get; init; } = string.Empty;

        public void AddDevelopers(IReadOnlyList<string> developers)
        {
            for (var index = 0; index < developers.Count; index++)
                _properties.Add($"{DeveloperPropertyPrefix}{index + 1}", developers[index]);
        }

        public void AddPublishers(IReadOnlyList<string> publishers)
        {
            for (var index = 0; index < publishers.Count; index++)
                _properties.Add($"{PublisherPropertyPrefix}{index + 1}", publishers[index]);
        }

        public void AddGenres(IReadOnlyList<string> genres)
        {
            for (var index = 0; index < genres.Count; index++)
                _properties.Add($"{GenrePropertyPrefix}{index + 1}", genres[index]);
        }

        public IDictionary<string, object> GetDeveloperProperties()
        {
            return _properties
                .Where(kvp => kvp.Key.StartsWith(DeveloperPropertyPrefix))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public IDictionary<string, object> GetPublisherProperties()
        {
            return _properties
                .Where(kvp => kvp.Key.StartsWith(PublisherPropertyPrefix))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public IDictionary<string, object> GetGenreProperties()
        {
            return _properties
                .Where(kvp => kvp.Key.StartsWith(GenrePropertyPrefix))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public GameReviewSummaryTableEntity WithUpdate(GameReviewSummaryForUpdate update)
        {
            var summary = new GameReviewSummaryTableEntity(this)
            {
                AverageUserRating = update.AverageUserRating,
                CoverArtLink = update.CoverArtLink,
                CoverArtThumbnailLink = update.CoverArtThumbnailLink,
                Description = update.Description,
                Engine = update.Engine,
                Series = update.Series,
            };

            summary.AddDevelopers(update.Developers);
            summary.AddGenres(update.Genres);
            summary.AddPublishers(update.Publishers);

            return summary;
        }

        public TableEntity ToTableEntity() => new(ToDictionaryForTableEntity());

        private IDictionary<string, object> ToDictionaryForTableEntity()
        {
            var properties = TypeDescriptor.GetProperties(this);

            foreach (PropertyDescriptor property in properties)
            {
                // ignore ETag
                if (property.Name == nameof(ETag))
                    continue;

#pragma warning disable CS8604 // Possible null reference argument.
                _properties.Add(property.Name, property.GetValue(this));
#pragma warning restore CS8604 // Possible null reference argument.
            }

            return _properties;
        }

        public static explicit operator GameReviewSummaryTableEntity(GameReviewSummaryForCreate summary) => new(summary);
        public static explicit operator GameReviewSummaryTableEntity(TableEntity summary) => new(summary);
    }
}
