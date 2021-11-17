using System.Collections.Generic;
using System.Linq;
using Bogus;

namespace TableStorage.CrudApi.Services.Games;

public interface IFakeSummaryGenerator
{
    IReadOnlyCollection<GameReviewSummaryTableEntity> GenerateSummaries(int count);
}

public sealed class FakeSummaryGenerator : IFakeSummaryGenerator
{
    private readonly IList<string> Platforms = new List<string>
    {
        "PS5",
        "XBoxSeriesX",
        "NinetendoSwitch",
        "PC"
    };

    public IReadOnlyCollection<GameReviewSummaryTableEntity> GenerateSummaries(int count)
    {
        var summaries = new Faker<GameReviewSummaryTableEntity>()
            .RuleFor(r => r.AverageUserRating, (f, r) => f.Random.Double(3, 10))
            .RuleFor(r => r.CoverArtLink, (f, r) => f.Image.LoremPixelUrl())
            .RuleFor(r => r.CoverArtThumbnailLink, (f, r) => f.Image.LoremPixelUrl())
            .RuleFor(r => r.Description, (f, r) => f.Lorem.Paragraph())
            .RuleFor(r => r.Engine, (f, r) => f.Lorem.Word())
            .RuleFor(r => r.GameId, (f, r) => f.Random.Hash())
            .RuleFor(r => r.Platform, (f, r) => f.PickRandom(Platforms))
            .RuleFor(r => r.Title, (f, r) => string.Join(" ", f.Lorem.Words()))
            .RuleFor(r => r.PartitionKey, (f, r) => r.Platform.ToLowerInvariant())
            .RuleFor(r => r.RowKey, (f, r) => $"{r.Title.ToLowerInvariant()}")
            .RuleFor(r => r.Type, _ => GameReviewSummaryTableEntity.TableEntityType)
            .Generate(count);

        var faker = new Faker();

        foreach (var summary in summaries)
        {
            var developers = Enumerable
                .Range(1, faker.Random.Int(2, 3))
                .Select(_ => faker.Company.CompanyName())
                .ToList();

            summary.AddDevelopers(developers);

            var publishers = Enumerable
                .Range(1, faker.Random.Int(2, 3))
                .Select(_ => faker.Company.CompanyName())
                .ToList();

            summary.AddPublishers(publishers);

            var genres = Enumerable
                .Range(1, faker.Random.Int(2, 3))
                .Select(_ => faker.Lorem.Word())
                .ToList();

            summary.AddGenres(genres);
        }

        return summaries;
    }
}
