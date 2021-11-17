using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TableStorage.CrudApi.Models;
using TableStorage.CrudApi.Services.Games;

namespace TableStorage.CrudApi.Controllers;

[ApiController]
[Route("game-review-summaries")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public sealed class GameReviewSummariesController : ControllerBase
{
    private readonly GameReviewSummaryService _service;

    public GameReviewSummariesController(GameReviewSummaryService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpPost]
    public async Task<IActionResult> AddGameReview(GameReviewSummaryForCreate summary)
    {
        var summaryFromStorage = await _service.AddGameReviewSummary(summary);

        return CreatedAtAction(
            actionName: nameof(Get),
            routeValues: new { platform = summaryFromStorage.Platform, title = summaryFromStorage.Title },
            value: summaryFromStorage);
    }

    [HttpPost("batch")]
    public async Task<IActionResult> AddGameReviewBatch(IReadOnlyCollection<GameReviewSummaryForCreate> summaries)
    {
        await _service.AddGameReviewSummaryBatch(summaries);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(
        [Required] string platform,
        [Required] string title)
    {
        var review = await _service.DeleteGameReviewSummary(platform, title);
        return review is null ? NotFound() : NoContent();
    }

    [HttpPost("fakes")]
    public async Task<IActionResult> GenerateFakes(int count)
    {
        await _service.GenerateData(count);
        return NoContent();
    }

    [HttpGet("{platform}", Name = nameof(Get))]
    public async Task<ActionResult<GameReviewSummary>> Get(string platform, [Required] string title)
    {
        var summary = await _service.GetGameReviewSummary(platform, title);
        return summary is null ? NotFound() : Ok(summary);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<GameReviewSummary>>> Get([FromQuery] GameReviewSummaryQuery query)
    {
        var summaries = await _service.GetGameReviewSummaries(query);
        return Ok(new PagedResponse<GameReviewSummary>(summaries));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateGameReview(GameReviewSummaryForUpdate suummary)
    {
        var update = await _service.UpdateGameReviewSummary(suummary);
        return update is null ? NotFound() : NoContent();
    }
}
