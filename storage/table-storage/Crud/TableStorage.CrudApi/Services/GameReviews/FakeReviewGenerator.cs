
using System;
using System.Collections.Generic;
using Bogus;

namespace TableStorage.CrudApi.Services.GameReviews;

public interface IFakeReviewGenerator
{
    IReadOnlyCollection<GameReviewTableEntity> GenerateReviews(int count);
}

public sealed class FakeReviewGenerator : IFakeReviewGenerator
{
    private readonly IList<string> _platforms = new List<string>
    {
        "PS5",
        "XBoxSeriesX",
        "NinetendoSwitch",
        "PC"
    };

    public IReadOnlyCollection<GameReviewTableEntity> GenerateReviews(int count)
    {
        return new Faker<GameReviewTableEntity>()
            .RuleFor(r => r.GameId, (f, r) => f.Random.Hash())
            .RuleFor(r => r.Platform, (f, r) => f.PickRandom(_platforms))
            .RuleFor(r => r.Title, (f, r) => string.Join(" ", f.Lorem.Words()))
            .RuleFor(r => r.PartitionKey, (f, r) => r.Platform.ToLowerInvariant())
            .RuleFor(r => r.Rating, (f, r) => f.Random.Number(3, 10))
            .RuleFor(r => r.Review, (f, r) => f.Lorem.Paragraph())
            .RuleFor(r => r.ReviewedAt, (f, r) => f.Date.PastOffset(1))
            .RuleFor(r => r.RowKey, (f, r) => $"{DateTime.MaxValue.Ticks - r.ReviewedAt.Ticks}")
            .RuleFor(r => r.Type, _ => GameReviewTableEntity.TableEntityType)
            .RuleFor(r => r.UserDisplayName, (f, r) => f.Internet.UserName(f.Name.FirstName(), f.Name.LastName()))
            .RuleFor(r => r.UserId, (f, r) => f.Random.Hash())
            .Generate(count);
    }
}
