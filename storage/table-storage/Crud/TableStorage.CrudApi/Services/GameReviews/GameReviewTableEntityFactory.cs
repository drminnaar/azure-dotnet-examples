using System.Collections.Generic;
using System.Linq;
using TableStorage.CrudApi.Services.Dates;

namespace TableStorage.CrudApi.Services.GameReviews;

public interface IGameReviewTableEntityFactory
{
    GameReviewTableEntity Create(GameReviewForCreate review);
}

public sealed class GameReviewTableEntityFactory : IGameReviewTableEntityFactory
{
    private readonly IDateTimeProvider _dateTime;

    public GameReviewTableEntityFactory(IDateTimeProvider dateTime)
    {
        _dateTime = dateTime ?? throw new System.ArgumentNullException(nameof(dateTime));
    }

    public GameReviewTableEntity Create(GameReviewForCreate review)
    {
        var reviewedAt = _dateTime.Now;
        return new GameReviewTableEntity
        {
            PartitionKey = review.Platform.ToLowerInvariant(),
            RowKey = $"{_dateTime.MaxValue.Ticks - reviewedAt.Ticks}",
            UserId = review.UserId,
            UserDisplayName = review.UserDisplayName,
            GameId = review.GameId,
            Platform = review.Platform,
            Title = review.Title,
            Rating = review.Rating,
            Review = review.Review,
            Type = GameReviewTableEntity.TableEntityType,
            ReviewedAt = reviewedAt,
        };
    }
}
