using Bogus;
using System.Collections.Generic;

namespace FakeData.Reviews
{
    public sealed class FakeReviewForCosmosGenerator : FakeEntityGeneratorBase<ReviewForCosmos>
    {
        public static readonly IList<string> ReviewTypes = new[] { "Game", "Music", "Movie", "Book" };

        public FakeReviewForCosmosGenerator() : base()
        {
        }

        public override IReadOnlyCollection<ReviewForCosmos> GenerateFakes(int reviewCount) =>
            new Faker<ReviewForCosmos>()
                .RuleFor(r => r.Id, (f, r) => f.Random.Hash())
                .RuleFor(r => r.ReviewId, (f, r) => f.Random.Hash())
                .RuleFor(r => r.Rating, (f, r) => f.Random.Number(1, 10))
                .RuleFor(r => r.Title, (f, r) => string.Join(" ", f.Lorem.Words()))
                .RuleFor(r => r.Type, (f, r) => f.PickRandom(ReviewTypes))
                .RuleFor(r => r.ReviewedBy, (f, r) => f.Internet.UserName(f.Name.FirstName(), f.Name.LastName()))
                .RuleFor(r => r.ReviewedAt, (f, r) => f.Date.PastOffset(1))
                .Generate(reviewCount);
    }
}
