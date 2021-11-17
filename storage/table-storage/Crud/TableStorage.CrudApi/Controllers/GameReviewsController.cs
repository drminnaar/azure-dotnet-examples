using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TableStorage.CrudApi.Models;
using TableStorage.CrudApi.Services.GameReviews;

namespace TableStorage.CrudApi.Controllers;

[ApiController]
[Route("game-reviews")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public sealed class GameReviewsController : ControllerBase
{
    private readonly GameReviewsService _service;

    public GameReviewsController(GameReviewsService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpPost]
    public async Task<IActionResult> AddGameReview(GameReviewForCreate review)
    {
        var reviewFromStorage = await _service.AddGameReview(review);
        return CreatedAtAction(
            actionName: nameof(GetReview),
            routeValues: new { reviewId = reviewFromStorage.ReviewId, platform = reviewFromStorage.Platform },
            value: reviewFromStorage);
    }

    [HttpPost("batch")]
    public async Task<IActionResult> AddGameReviewBatch(IReadOnlyCollection<GameReviewForCreate> reviews)
    {
        await _service.AddGameReviewBatch(reviews);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(
        [Required] string platform,
        [Required] string reviewId)
    {
        var review = await _service.DeleteGameReview(platform, reviewId);
        return review is null ? NotFound() : NoContent();
    }

    [HttpPost("fakes")]
    public async Task<IActionResult> GenerateFakeReviews(int count)
    {
        await _service.GenerateData(count);
        return NoContent();
    }

    [HttpGet("{reviewId}", Name = nameof(GetReview))]
    public async Task<ActionResult<GameReview>> GetReview(string reviewId, [Required] string platform)
    {
        var review = await _service.GetGameReview(platform, reviewId);
        return review is null ? NotFound() : Ok(review);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<GameReview>>> Get([FromQuery] GameReviewQuery query)
    {
        var reviews = await _service.GetGameReviews(query);
        return Ok(new PagedResponse<GameReview>(reviews));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateGameReview(GameReviewForUpdate review)
    {
        var update = await _service.UpdateGameReview(review);
        return update is null ? NotFound() : NoContent();
    }
}
