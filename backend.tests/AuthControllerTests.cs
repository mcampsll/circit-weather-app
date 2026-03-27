using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Xunit;
using backend.Controllers;

namespace backend.tests;

public class AuthControllerTests
{
    // ---------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------

    private static AuthController BuildController(
        string tokenResponse  = "access_token=test_token&token_type=bearer",
        string userResponse   = """{"login":"testuser","name":"Test User"}""",
        HttpStatusCode status = HttpStatusCode.OK)
    {
        var callCount = 0;
        var handler   = new Mock<HttpMessageHandler>();
        handler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() =>
            {
                callCount++;
                // First call = token exchange, second call = user profile
                return new HttpResponseMessage
                {
                    StatusCode = status,
                    Content    = new StringContent(callCount == 1 ? tokenResponse : userResponse)
                };
            });

        var factory = new Mock<IHttpClientFactory>();
        factory.Setup(f => f.CreateClient(It.IsAny<string>()))
               .Returns(new HttpClient(handler.Object));

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["GitHub:ClientId"]     = "test-client-id",
                ["GitHub:ClientSecret"] = "test-client-secret",
                ["GitHub:RedirectUri"]  = "http://localhost:5000/auth/callback",
                ["GitHub:FrontendUrl"]  = "http://localhost:5173",
            })
            .Build();

        var controller = new AuthController(factory.Object, config);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        return controller;
    }

    // ---------------------------------------------------------------
    // Login
    // ---------------------------------------------------------------

    [Fact]
    public void Login_ReturnsRedirectToGitHub()
    {
        var controller = BuildController();
        var result     = controller.Login();

        var redirect = Assert.IsType<RedirectResult>(result);
        Assert.StartsWith("https://github.com/login/oauth/authorize", redirect.Url);
    }

    [Fact]
    public void Login_RedirectUrl_ContainsClientId()
    {
        var controller = BuildController();
        var result     = (RedirectResult)controller.Login();

        Assert.Contains("client_id=test-client-id", result.Url);
    }

    [Fact]
    public void Login_RedirectUrl_ContainsScope()
    {
        var controller = BuildController();
        var result     = (RedirectResult)controller.Login();

        Assert.Contains("scope=", result.Url);
    }

    // ---------------------------------------------------------------
    // Callback
    // ---------------------------------------------------------------

    [Fact]
    public async Task Callback_MissingCode_Returns400()
    {
        var controller = BuildController();
        var result     = await controller.Callback(string.Empty);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Callback_NullCode_Returns400()
    {
        var controller = BuildController();
        var result     = await controller.Callback(null!);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Callback_ValidCode_RedirectsToFrontend()
    {
        var controller = BuildController();
        var result     = await controller.Callback("valid-code");

        var redirect = Assert.IsType<RedirectResult>(result);
        Assert.StartsWith("http://localhost:5173", redirect.Url);
    }

    [Fact]
    public async Task Callback_ValidCode_RedirectContainsUserParam()
    {
        var controller = BuildController();
        var result     = await controller.Callback("valid-code");

        var redirect = Assert.IsType<RedirectResult>(result);
        Assert.Contains("user=", redirect.Url);
    }

    [Fact]
    public async Task Callback_TokenExchangeFails_RedirectsWithError()
    {
        var controller = BuildController(tokenResponse: "error=bad_verification_code");
        var result     = await controller.Callback("bad-code");

        var redirect = Assert.IsType<RedirectResult>(result);
        Assert.Contains("error=token_exchange_failed", redirect.Url);
    }
}