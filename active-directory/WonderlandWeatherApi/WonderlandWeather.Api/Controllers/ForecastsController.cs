using Microsoft.AspNetCore.Mvc;
using WonderlandWeather.Api.Models;

namespace WonderlandWeather.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ForecastsController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet]
    public IEnumerable<Forecast> Get()
    {
        return Enumerable
            .Range(1, 5)
            .Select(index => new Forecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}
