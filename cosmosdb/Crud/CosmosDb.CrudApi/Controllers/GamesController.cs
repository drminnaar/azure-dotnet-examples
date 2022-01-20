using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using Azure.Cosmos;
using CosmosDb.CrudApi.Services.Games;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CosmosDb.CrudApi.Controllers;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Route("games")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
    }

    [HttpPost(Name = nameof(AddGame))]
    public async Task<IActionResult> AddGame(GameForCreate gameForCreate)
    {
        var game = await _gameService.AddGame(gameForCreate);
        return CreatedAtAction(nameof(GetGame), new { game.Id, game.Platform }, game);
    }

    [HttpDelete("{id}", Name = nameof(DeleteGame))]
    public async Task<IActionResult> DeleteGame(string id, [Required] string platform)
    {
        var game = await _gameService.DeleteGame(id, platform);
        return game is null ? NotFound() : NoContent();
    }

    [HttpPost("fakes", Name = nameof(GenerateFakeGameData))]
    public async Task<IActionResult> GenerateFakeGameData(int fakes = 100)
    {
        await _gameService.GenerateData(fakes);
        return NoContent();
    }

    [HttpGet("{id}", Name = nameof(GetGame))]
    [ProducesResponseType(typeof(GameForQuery), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetGame(string id, string platform)
    {
        var game = await _gameService.GetGame(id, platform);

        return (game is null) ? NotFound() : Ok(game);
    }

    [HttpGet(Name = nameof(GetGames))]
    public async Task<IActionResult> GetGames(
        [Required] string platform,
        int pageNumber,
        int pageSize)
    {
        var games = await _gameService.GetGames(new CosmosQueryInput
        {
            PartitionKey = GamePartitionKey.Create(platform).ToString(),
            PageSize = pageSize,
            PageNumber = pageNumber
        });

        return Ok(games);
    }

    [HttpPut(Name = nameof(UpdateGame))]
    public async Task<IActionResult> UpdateGame(string id, GameForUpdate gameForUpdate)
    {
        var game = await _gameService.UpdateGame(id, gameForUpdate);

        return (game is null) ? NotFound() : Ok(game);
    }
}
