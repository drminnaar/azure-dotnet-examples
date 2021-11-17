
using System;

namespace TableStorage.CrudApi.Services.GameReviews;

public sealed record GameReview
{
    public GameReview()
    {
    }

    public GameReview(GameReviewTableEntity review)
    {
        ReviewId = review.RowKey;
        UserId = review.UserId;
        UserDisplayName = review.UserDisplayName;
        GameId = review.GameId;
        Platform = review.Platform;
        Title = review.Title;
        Rating = review.Rating;
        Review = review.Review;
        ReviewedAt = review.ReviewedAt;
    }

    public string ReviewId { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public string UserDisplayName { get; init; } = string.Empty;
    public string GameId { get; init; } = string.Empty;
    public string Platform { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public int? Rating { get; init; }
    public string? Review { get; init; } = string.Empty;
    public DateTimeOffset ReviewedAt { get; init; }

    public static explicit operator GameReview(GameReviewTableEntity review) => new(review);
}
