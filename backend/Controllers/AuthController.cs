using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IHttpClientFactory httpClientFactory, IConfiguration config) : ControllerBase
{
    [HttpGet("login")]
    public IActionResult Login()
    {
        var url = $"https://github.com/login/oauth/authorize" +
                  $"?client_id={config["GitHub:ClientId"]}" +
                  $"&redirect_uri={config["GitHub:RedirectUri"]}" +
                  $"&scope=read:user user:email";
        return Redirect(url);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        if (string.IsNullOrEmpty(code))
            return BadRequest(new { error = "Missing code" });

        var frontendUrl = config["GitHub:FrontendUrl"];
        var client = httpClientFactory.CreateClient();

        // Exchange code for token
        var tokenRes = await client.PostAsJsonAsync(
            "https://github.com/login/oauth/access_token",
            new { client_id = config["GitHub:ClientId"], client_secret = config["GitHub:ClientSecret"], code });

        var tokenRaw = await tokenRes.Content.ReadAsStringAsync();
        var token = tokenRaw.Split('&')
            .FirstOrDefault(p => p.StartsWith("access_token="))
            ?.Replace("access_token=", "");

        if (string.IsNullOrEmpty(token))
            return Redirect($"{frontendUrl}/auth?error=token_exchange_failed");

        // Fetch GitHub user profile
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        client.DefaultRequestHeaders.Add("User-Agent", "CircitWeatherApp");

        var userRes = await client.GetAsync("https://api.github.com/user");
        var userJson = await userRes.Content.ReadAsStringAsync();

        return Redirect($"{frontendUrl}/auth?user={Uri.EscapeDataString(userJson)}");
    }
}