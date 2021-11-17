
namespace TableStorage.CrudApi.Services.GameReviews;

public sealed record GameReviewForUpdate
{
    public string ReviewId { get; init; } = string.Empty;
    public string Platform { get; init; } = string.Empty;
    public int? Rating { get; init; }
    public string? Review { get; init; } = string.Empty;
}
