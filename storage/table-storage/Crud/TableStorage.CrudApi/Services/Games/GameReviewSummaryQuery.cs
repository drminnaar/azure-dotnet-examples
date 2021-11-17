using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TableStorage.CrudApi.Services.Games;

public sealed record GameReviewSummaryQuery
{
    [FromQuery(Name = "page")]
    public int PageNumber { get; set; } = 1;

    [FromQuery(Name = "limit")]
    public int PageSize { get; set; } = 25;

    [Required(AllowEmptyStrings = false, ErrorMessage = "The 'platform' parameter is required")]
    public string Platform { get; set; } = string.Empty;

    public string? Title { get; set; } = string.Empty;

    [FromQuery(Name = "max-rating")]
    public double? MaxAverageUserRating { get; set; }

    [FromQuery(Name = "min-rating")]
    public double? MinAverageUserRating { get; set; }
}
