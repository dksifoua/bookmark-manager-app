using System.Net;
using FluentAssertions;
using JsonContent = System.Net.Http.Json.JsonContent;

namespace BookmarkManagerApp.IntegrationTests.controllers;

public class AuthControllerTest(BookmarkManagerAppFactory factory) : IClassFixture<BookmarkManagerAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    private const string BaseUrl = "/api/auth";

    private async Task<string> LoginAndGetJwtCookieAsync(string email, string password)
    {
        using var loginRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/login");
        var loginRequestData = new
        {
            Email = email,
            Password = password
        };
        loginRequest.Content = JsonContent.Create(loginRequestData);

        using var loginResponse = await _client.SendAsync(loginRequest, _cancellationToken);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        return loginResponse.Headers.TryGetValues("Set-Cookie", out var values)
            ? values.First()
            : throw new InvalidOperationException("Login response did not contain Set-Cookie header.");
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsCreated()
    {
        // Arrange
        using var registerRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/register");
        var registerRequestData = new
        {
            Fullname = "Valid User",
            Email = "valid.user@example.com",
            Password = "Test123!"
        };
        registerRequest.Content = JsonContent.Create(registerRequestData);

        // Act
        using var registerResponse = await _client.SendAsync(registerRequest, _cancellationToken);

        // Assert
        registerResponse.Should().Be201Created()
            .And.BeAs(new { registerRequestData.Fullname, registerRequestData.Email })
            .And.HaveHeader("Location")
            .And.Match("http://localhost/api/users/2");

        // Arrange
        var validJwtCookie = await LoginAndGetJwtCookieAsync(registerRequestData.Email, registerRequestData.Password);
        using var getUserRequest = new HttpRequestMessage(HttpMethod.Get, "api/users/2");
        getUserRequest.Headers.Add("Cookie", validJwtCookie);

        // Act
        using var getUserResponse = await _client.SendAsync(getUserRequest, _cancellationToken);

        // Assert
        getUserResponse.Should().Be200Ok()
            .And.BeAs(new { registerRequestData.Fullname, registerRequestData.Email });
    }

    [Fact]
    public async Task Register_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        using var registerRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/register");
        var registerRequestData = new
        {
            Fullname = "",
            Email = "email",
            Password = "Pass123"
        };
        registerRequest.Content = JsonContent.Create(registerRequestData);

        // Act
        using var registerResponse = await _client.SendAsync(registerRequest, _cancellationToken);

        // Assert
        registerResponse.Should().Be400BadRequest()
            .And.BeAs(new
            {
                title = "Validation Error",
                status = 400,
                errors = new
                {
                    Fullname = (string[])
                    [
                        "'Fullname' must not be empty.",
                        "The length of 'Fullname' must be at least 3 characters. You entered 0 characters."
                    ],
                    Email = (string[])["'Email' is not a valid email address."],
                    Password = (string[])
                        ["The length of 'Password' must be at least 8 characters. You entered 7 characters."]
                }
            });
    }

    [Fact]
    public async Task Register_WithExistingData_ReturnsConflict()
    {
        // Arrange
        using var registerRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/register");
        var registerRequestData = new
        {
            Fullname = "Test User",
            Email = "test.user@example.com",
            Password = "Pass123!"
        };
        registerRequest.Content = JsonContent.Create(registerRequestData);

        // Act
        using var registerResponse = await _client.SendAsync(registerRequest, _cancellationToken);

        // Assert
        registerResponse.Should().Be409Conflict();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOk()
    {
        // Arrange
        using var loginRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/login");
        var loginRequestData = new
        {
            Email = "test.user@example.com",
            Password = "Pass123!"
        };
        loginRequest.Content = JsonContent.Create(loginRequestData);

        // Act
        var loginResponse = await _client.SendAsync(loginRequest, _cancellationToken);

        // Assert
        loginResponse.Should().Be200Ok()
            .And.BeAs(new { Fullname = "Test User", loginRequestData.Email })
            .And.HaveHeader("Set-Cookie")
            .And.Match("token=*; max-age=3600; path=/; samesite=strict; httponly");
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        using var loginRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/login");
        var loginRequestData = new
        {
            Email = "invalid.user@example.com",
            Password = "WrongPassword123!"
        };
        loginRequest.Content = JsonContent.Create(loginRequestData);

        // Act
        using var loginResponse = await _client.SendAsync(loginRequest, _cancellationToken);

        // Assert
        loginResponse.Should().Be401Unauthorized();
    }

    [Fact]
    public async Task Logout_ReturnsOk()
    {
        // Arrange
        var validJwtCookie = await LoginAndGetJwtCookieAsync("test.user@example.com", "Pass123!");

        using var logoutRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/logout");
        logoutRequest.Headers.Add("Cookie", validJwtCookie);

        // Act
        using var logoutResponse = await _client.SendAsync(logoutRequest, _cancellationToken);

        // Assert
        logoutResponse.Should().Be204NoContent()
            .And.HaveHeader("Set-Cookie")
            .And.Match("token=; expires=*; path=/; samesite=strict; httponly");
    }
}