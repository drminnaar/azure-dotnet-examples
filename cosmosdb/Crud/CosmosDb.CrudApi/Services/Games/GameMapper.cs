using System;
using System.Collections.Generic;
using Azure.Cosmos;

namespace CosmosDb.CrudApi.Services.Games;

public interface IGameMapper
{
    GameEntity MapToGameEntity(GameForCreate game);
    GameEntity MapToGameEntity(GameForUpdate gameUpdate, GameEntity gameOriginal);
    GameForQuery MapToGameForQuery(GameEntity game);
}

public sealed class GameMapper : IGameMapper
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICosmosEntityIdProvider _entityIdProvider;

    public GameMapper(IDateTimeProvider dateTimeProvider, ICosmosEntityIdProvider entityIdProvider)
    {
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        _entityIdProvider = entityIdProvider ?? throw new ArgumentNullException(nameof(entityIdProvider));
    }

    public GameEntity MapToGameEntity(GameForCreate game)
    {
        return new GameEntity
        {
            CoverArtLink = game.CoverArtLink,
            CoverArtThumbnailLink = game.CoverArtThumbnailLink,
            Description = game.Description,
            Engine = game.Engine,
            GameId = game.GameId,
            Pk = GamePartitionKey.Create(game).ToString(),
            Platform = game.Platform,
            Series = game.Series,
            Title = game.Title,
            Type = GameEntity.EntityType.ToLowerInvariant(),
            Genres = new List<string>(game.Genres),
            Developers = new List<string>(game.Developers),
            Publishers = new List<string>(game.Publishers),
            CreatedAt = _dateTimeProvider.Now,
            UpdatedAt = _dateTimeProvider.Now,
            Id = _entityIdProvider.GenerateId()
        };
    }

    public GameEntity MapToGameEntity(GameForUpdate gameUpdate, GameEntity gameOriginal)
    {
        return new GameEntity
        {
            Id = gameOriginal.Id,
            CoverArtLink = gameUpdate.CoverArtLink,
            CoverArtThumbnailLink = gameUpdate.CoverArtThumbnailLink,
            Description = gameUpdate.Description,
            Developers = new List<string>(gameUpdate.Developers),
            Engine = gameUpdate.Engine,
            GameId = gameOriginal.Id,
            Genres = new List<string>(gameUpdate.Genres),
            Pk = gameOriginal.Pk,
            Platform = gameUpdate.Platform,
            Publishers = new List<string>(gameUpdate.Publishers),
            Series = gameUpdate.Series,
            Title = gameUpdate.Title,
            Type = gameOriginal.Type,
            UpdatedAt = _dateTimeProvider.Now
        };
    }

    public GameForQuery MapToGameForQuery(GameEntity game) => new()
    {
        CoverArtLink = game.CoverArtLink,
        CoverArtThumbnailLink = game.CoverArtThumbnailLink,
        CreatedAt = game.CreatedAt,
        Description = game.Description,
        Developers = game.Developers,
        Engine = game.Engine,
        GameId = game.GameId,
        Genres = game.Genres,
        Id = game.Id,
        Platform = game.Platform,
        Publishers = game.Publishers,
        Series = game.Series,
        Title = game.Title,
        UpdatedAt = game.UpdatedAt,
    };
}
