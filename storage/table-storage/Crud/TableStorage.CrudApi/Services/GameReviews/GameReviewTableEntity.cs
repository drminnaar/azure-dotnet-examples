
using System;
using Azure;
using Azure.Data.Tables;

namespace TableStorage.CrudApi.Services.GameReviews;

public sealed record GameReviewTableEntity : ITableEntity
{
    public const string TableEntityType = "GameReview";

    public GameReviewTableEntity()
    {
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="review"></param>
    public GameReviewTableEntity(GameReviewTableEntity review)
    {
        PartitionKey = review.PartitionKey;
        RowKey = review.RowKey;
        Timestamp = review.Timestamp;
        ETag = review.ETag;
        UserId = review.UserId;
        UserDisplayName = review.UserDisplayName;
        GameId = review.GameId;
        Platform = review.Platform;
        Title = review.Title;
        Rating = review.Rating;
        Review = review.Review;
        Type = review.Type;
        ReviewedAt = review.ReviewedAt;
    }

    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public string UserId { get; init; } = string.Empty;
    public string UserDisplayName { get; init; } = string.Empty;
    public string GameId { get; init; } = string.Empty;
    public string Platform { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public int? Rating { get; init; }
    public string? Review { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public DateTimeOffset ReviewedAt { get; init; }

    public GameReviewTableEntity WithUpdate(GameReviewForUpdate update)
    {
        return new GameReviewTableEntity(this)
        {
            Rating = update.Rating,
            Review = update.Review
        };
    }
}
