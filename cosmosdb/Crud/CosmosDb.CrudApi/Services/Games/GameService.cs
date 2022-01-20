using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Azure.Cosmos;
using Microsoft.AspNetCore.Http;

namespace CosmosDb.CrudApi.Services.Games;

public interface IGameService
{
    Task<GameForQuery> AddGame(GameForCreate gameForCreate);
    Task<GameForQuery?> DeleteGame(string id, string platform);
    Task GenerateData(int count);
    Task<GameForQuery?> GetGame(string id, string platform);
    Task<IPagedCollection<GameForQuery>> GetGames(CosmosQueryInput input);
    Task<GameForQuery?> UpdateGame(string id, GameForUpdate gameForUpdate);
}

public sealed class GameService : IGameService
{
    private readonly IGameFaker _faker;
    private readonly IGameMapper _mapper;
    private readonly ICosmosContainerWrapper _container;

    public GameService(
        IGameFaker gameFaker,
        IGameMapper mapper,
        ICosmosContainerWrapper cosmosContainer)
    {
        _faker = gameFaker ?? throw new ArgumentNullException(nameof(gameFaker));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _container = cosmosContainer;
    }

    public async Task<GameForQuery> AddGame(GameForCreate gameForCreate)
    {
        var game = _mapper.MapToGameEntity(gameForCreate);

        await _container.CreateItemAsync(game, GamePartitionKey.Value(gameForCreate));

        return _mapper.MapToGameForQuery(game);
    }

    public async Task<GameForQuery?> DeleteGame(string id, string platform)
    {
        try
        {
            var game = await _container.DeleteItemAsync<GameEntity>(id, GamePartitionKey.Value(platform));

            return _mapper.MapToGameForQuery(game);
        }
        catch (CosmosException ex) when (ex.Status == StatusCodes.Status404NotFound)
        {
            return null;
        }
    }

    public async Task GenerateData(int count)
    {
        var games = _faker.GenerateGames(count);

        var tasks = new List<Task>();

        foreach (var game in games)
        {
            tasks.Add(_container
                .CreateItemAsync(game, GamePartitionKey.Value(game.Platform))
                .ContinueWith(response =>
                {
                    if (!response.IsCompletedSuccessfully)
                    {
                        var exception = response
                            .Exception
                            .Flatten()
                            .InnerExceptions
                            .FirstOrDefault();

                        Debug.WriteLine(exception);
                    }
                }));
        }
        await Task.WhenAll(tasks);
    }

    public async Task<GameForQuery?> GetGame(string id, string platform)
    {
        try
        {
            var gameFromDb = await _container
                .ReadItemAsync<GameEntity>(id, GamePartitionKey.Value(platform));

            return _mapper.MapToGameForQuery(gameFromDb);
        }
        catch (CosmosException ex) when (ex.Status == StatusCodes.Status404NotFound)
        {
            return null;
        }
    }

    public async Task<IPagedCollection<GameForQuery>> GetGames(CosmosQueryInput input)
    {
        var pagedCollection = await _container.ReadItemsAsync<GameEntity>(input);

        return new PagedCollection<GameForQuery>(
            pagedCollection.Select(entity => _mapper.MapToGameForQuery(entity)).ToList(),
            pagedCollection.ItemCount,
            pagedCollection.CurrentPageNumber,
            pagedCollection.PageSize);
    }

    public async Task<GameForQuery?> UpdateGame(string entityId, GameForUpdate gameForUpdate)
    {
        GameEntity gameFromDb;

        try
        {
            gameFromDb = await _container.ReadItemAsync<GameEntity>(
                entityId,
                GamePartitionKey.Value(gameForUpdate));
        }
        catch (CosmosException ex) when (ex.Status == StatusCodes.Status404NotFound)
        {
            return null;
        }

        var gameWithUpdates = _mapper.MapToGameEntity(gameForUpdate, gameFromDb);

        await _container.UpsertItemAsync(gameWithUpdates, GamePartitionKey.Value(gameForUpdate));

        return _mapper.MapToGameForQuery(gameWithUpdates);
    }
}
