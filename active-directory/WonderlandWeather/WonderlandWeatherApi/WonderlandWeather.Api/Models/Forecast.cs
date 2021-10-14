namespace WonderlandWeather.Api.Models;

public sealed record Forecast
{
    public string Id { get; init; } = string.Empty;

    public DateTime Date { get; init; }

    public int TemperatureC { get; init; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; init; }
}
