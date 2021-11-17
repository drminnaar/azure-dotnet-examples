
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TableStorage.CrudApi.Services.GameReviews;

public sealed record GameReviewQuery
{
    [FromQuery(Name = "page")]
    public int PageNumber { get; set; } = 1;

    [FromQuery(Name = "limit")]
    public int PageSize { get; set; } = 25;

    [Required(AllowEmptyStrings = false, ErrorMessage = "The 'platform' parameter is required")]
    public string Platform { get; set; } = string.Empty;

    public string? UserId { get; set; } = string.Empty;

    public string? Title { get; set; } = string.Empty;

    public DateTimeOffset? From { get; set; }

    public DateTimeOffset? To { get; set; }
}
