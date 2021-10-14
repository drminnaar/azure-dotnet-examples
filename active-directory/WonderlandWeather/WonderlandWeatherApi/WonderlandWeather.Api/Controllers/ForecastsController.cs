using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using WonderlandWeather.Api.Models;

namespace WonderlandWeather.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public sealed class ForecastsController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public IEnumerable<Forecast> Get()
    {
        return Enumerable
            .Range(1, 5)
            .Select(index => new Forecast
            {
                Id = Guid.NewGuid().ToString(),
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [AllowAnonymous]
    [HttpOptions]
    public IActionResult GetOptions()
    {
        Response.Headers.Add("Allow", "GET,OPTIONS");
        return NoContent();
    }
}
