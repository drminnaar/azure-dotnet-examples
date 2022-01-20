using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;

namespace CosmosDb.CrudApi.Services.Games;

public interface IGameFaker
{
    IReadOnlyCollection<GameEntity> GenerateGames(int count);
}

public sealed class GameFaker : IGameFaker
{
    private readonly IList<string> _platforms = new List<string>
        {
            "PS5",
            "XBoxSeriesX",
            "NinetendoSwitch",
            "PC"
        };

    public IReadOnlyCollection<GameEntity> GenerateGames(int count)
    {
        var games = new Faker<GameEntity>()
            .RuleFor(r => r.CoverArtLink, (f, r) => f.Image.LoremPixelUrl())
            .RuleFor(r => r.CoverArtThumbnailLink, (f, r) => f.Image.LoremPixelUrl())
            .RuleFor(r => r.Description, (f, r) => f.Lorem.Paragraph())
            .RuleFor(r => r.Engine, (f, r) => f.Lorem.Word())
            .RuleFor(r => r.GameId, (f, r) => f.Random.Hash())
            .RuleFor(r => r.Id, (f, r) => f.Random.Hash())
            .RuleFor(r => r.Platform, (f, r) => f.PickRandom(_platforms))
            .RuleFor(r => r.Title, (f, r) => string.Join(" ", f.Lorem.Words()))
            .RuleFor(r => r.Pk, (f, r) => GamePartitionKey.Create(r.Platform).ToString())
            .RuleFor(r => r.Type, _ => GameEntity.EntityType)
            .RuleFor(r => r.CreatedAt, _ => DateTimeOffset.Now)
            .RuleFor(r => r.UpdatedAt, _ => DateTimeOffset.Now)
            .Generate(count);

        var faker = new Faker();

        foreach (var game in games)
        {
            var developers = Enumerable
                .Range(1, faker.Random.Int(2, 3))
                .Select(_ => faker.Company.CompanyName())
                .ToList();

            game.Developers.AddRange(developers);

            var publishers = Enumerable
                .Range(1, faker.Random.Int(2, 3))
                .Select(_ => faker.Company.CompanyName())
                .ToList();

            game.Publishers.AddRange(publishers);

            var genres = Enumerable
                .Range(1, faker.Random.Int(2, 3))
                .Select(_ => faker.Lorem.Word())
                .ToList();

            game.Genres.AddRange(genres);
        }

        return games;
    }
}
