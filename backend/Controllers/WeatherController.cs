using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController(IHttpClientFactory httpClientFactory, IConfiguration config) : ControllerBase
{
    private static readonly HashSet<string> AllowedCities =
        new(["dublin", "barcelona", "anchorage"], StringComparer.OrdinalIgnoreCase);

    private HttpClient CreateClient()
    {
        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("X-RapidAPI-Key", config["RapidApi:Key"]);
        client.DefaultRequestHeaders.Add("X-RapidAPI-Host", config["RapidApi:Host"]);
        return client;
    }

    private string BaseUrl => config["RapidApi:BaseUrl"]!;

    [HttpGet("cities")]
    public async Task<IActionResult> GetAllCities()
    {
        var client = CreateClient();
        var tasks = AllowedCities.Select(city => client.GetAsync($"{BaseUrl}/current.json?q={city}")).ToArray();
        var responses = await Task.WhenAll(tasks);

        if (responses.Any(r => !r.IsSuccessStatusCode))
            return StatusCode(502, new { error = "Error fetching cities data" });

        var contents = await Task.WhenAll(responses.Select(r => r.Content.ReadAsStringAsync()));
        var cities = AllowedCities.ToArray();

        return Ok(cities.Select((city, i) => new
        {
            city,
            data = System.Text.Json.JsonSerializer.Deserialize<object>(contents[i])
        }));
    }

    [HttpGet("all/{city}")]
    public async Task<IActionResult> GetAll(string city)
    {
        if (!AllowedCities.Contains(city))
            return BadRequest(new { error = "City not allowed" });

        var client = CreateClient();
        var tasks = new[]
        {
            client.GetAsync($"{BaseUrl}/current.json?q={city}"),
            client.GetAsync($"{BaseUrl}/astronomy.json?q={city}")
        };
        var responses = await Task.WhenAll(tasks);

        if (responses.Any(r => !r.IsSuccessStatusCode))
            return StatusCode(502, new { error = "Error fetching data from weather service" });

        var contents = await Task.WhenAll(responses.Select(r => r.Content.ReadAsStringAsync()));

        return Ok(new
        {
            current  = System.Text.Json.JsonSerializer.Deserialize<object>(contents[0]),
            astronomy = System.Text.Json.JsonSerializer.Deserialize<object>(contents[1])
        });
    }
}