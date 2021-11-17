
namespace TableStorage.CrudApi.Services.GameReviews;

public sealed record GameReviewForCreate
{
    public string UserId { get; init; } = string.Empty;
    public string UserDisplayName { get; init; } = string.Empty;
    public string GameId { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Platform { get; init; } = string.Empty;
    public int? Rating { get; init; }
    public string? Review { get; init; } = string.Empty;
}
