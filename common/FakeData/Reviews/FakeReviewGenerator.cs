using Bogus;
using System.Collections.Generic;

namespace FakeData.Reviews
{
    public sealed class FakeReviewGenerator : FakeEntityGeneratorBase<Review>
    {
        public static readonly IList<string> ReviewTypes = new[] { "Game", "Music", "Movie", "Book" };

        public FakeReviewGenerator() : base()
        {
        }

        public override IReadOnlyCollection<Review> GenerateFakes(int reviewCount) =>
            new Faker<Review>()
                .RuleFor(r => r.Id, (f, r) => f.Random.Hash())
                .RuleFor(r => r.Rating, (f, r) => f.Random.Number(1, 10))
                .RuleFor(r => r.Title, (f, r) => string.Join(" ", f.Lorem.Words()))
                .RuleFor(r => r.Type, (f, r) => f.PickRandom(ReviewTypes))
                .RuleFor(r => r.ReviewedBy, (f, r) => f.Internet.UserName(f.Name.FirstName(), f.Name.LastName()))
                .Generate(reviewCount);
    }
}
