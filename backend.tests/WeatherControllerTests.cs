using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Xunit;
using backend.Controllers;

namespace backend.tests;

public class WeatherControllerTests
{
    // ---------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------

    private static WeatherController BuildController(
        HttpStatusCode statusCode,
        string responseBody = "{}")
    {
        var handler = new Mock<HttpMessageHandler>();
        handler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content    = new StringContent(responseBody)
            });

        var factory = new Mock<IHttpClientFactory>();
        factory.Setup(f => f.CreateClient(It.IsAny<string>()))
               .Returns(new HttpClient(handler.Object));

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["RapidApi:Key"]     = "test-key",
                ["RapidApi:Host"]    = "weatherapi-com.p.rapidapi.com",
                ["RapidApi:BaseUrl"] = "https://weatherapi-com.p.rapidapi.com",
            })
            .Build();

        return new WeatherController(factory.Object, config);
    }

    // ---------------------------------------------------------------
    // GetAll
    // ---------------------------------------------------------------

    [Fact]
    public async Task GetAll_ValidCity_Returns200()
    {
        var controller = BuildController(HttpStatusCode.OK, "{}");
        var result     = await controller.GetAll("dublin");

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetAll_InvalidCity_Returns400()
    {
        var controller = BuildController(HttpStatusCode.OK);
        var result     = await controller.GetAll("madrid");

        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(bad.Value);
    }

    [Theory]
    [InlineData("dublin")]
    [InlineData("barcelona")]
    [InlineData("anchorage")]
    [InlineData("DUBLIN")]       // case-insensitive
    [InlineData("Barcelona")]
    public async Task GetAll_AllAllowedCities_Return200(string city)
    {
        var controller = BuildController(HttpStatusCode.OK, "{}");
        var result     = await controller.GetAll(city);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetAll_UpstreamError_Returns502()
    {
        var controller = BuildController(HttpStatusCode.InternalServerError);
        var result     = await controller.GetAll("dublin");

        var status = Assert.IsType<ObjectResult>(result);
        Assert.Equal(502, status.StatusCode);
    }

    // ---------------------------------------------------------------
    // GetAllCities
    // ---------------------------------------------------------------

    [Fact]
    public async Task GetAllCities_Success_ReturnsOk()
    {
        var controller = BuildController(HttpStatusCode.OK, "{}");
        var result     = await controller.GetAllCities();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetAllCities_UpstreamError_Returns502()
    {
        var controller = BuildController(HttpStatusCode.TooManyRequests);
        var result     = await controller.GetAllCities();

        var status = Assert.IsType<ObjectResult>(result);
        Assert.Equal(502, status.StatusCode);
    }
}