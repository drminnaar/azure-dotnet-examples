using System;
using System.Linq.Expressions;

namespace TableStorage.CrudApi.Services.GameReviews;

public sealed class GameReviewQueryBuilder
{
    public Expression<Func<GameReviewTableEntity, bool>> Filter { get; private set; } = ExpressionBuilder
        .True<GameReviewTableEntity>()
        .And(review => review.Type == nameof(GameReview));

    public GameReviewQueryBuilder WherePlatformEquals(string? platform)
    {
        if (!string.IsNullOrWhiteSpace(platform))
            Filter = Filter.And(review => review.PartitionKey == platform.ToLowerInvariant());

        return this;
    }

    public GameReviewQueryBuilder WhereMaxReviewedAtEquals(DateTimeOffset? date)
    {
        if (date.HasValue)
            Filter = Filter.And(review => review.ReviewedAt <= date);

        return this;
    }

    public GameReviewQueryBuilder WhereMinReviewedAtEquals(DateTimeOffset? date)
    {
        if (date.HasValue)
            Filter = Filter.And(review => review.ReviewedAt >= date);

        return this;
    }

    public GameReviewQueryBuilder WhereTitleEquals(string? title)
    {
        if (!string.IsNullOrWhiteSpace(title))
            Filter = Filter.And(review => review.Title == title);

        return this;
    }

    public GameReviewQueryBuilder WhereUserIdEquals(string? userId)
    {
        if (!string.IsNullOrWhiteSpace(userId))
            Filter = Filter.And(review => review.UserId == userId);

        return this;
    }
}
