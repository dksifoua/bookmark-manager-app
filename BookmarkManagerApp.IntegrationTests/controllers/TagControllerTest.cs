using FluentAssertions;

namespace BookmarkManagerApp.IntegrationTests.controllers;

public class TagControllerTest : IClassFixture<BookmarkManagerAppFactory>
{
    private const string BaseUrl = "/api/tags";

    private readonly HttpClient _client;
    private readonly CancellationToken _cancellationToken;
    private readonly Utility _utility;

    public TagControllerTest(BookmarkManagerAppFactory factory)
    {
        _client = factory.CreateClient();
        _cancellationToken = CancellationToken.None;
        _utility = new Utility(_client, _cancellationToken);
    }

    [Fact]
    public async Task RetrieveAllAsync_WithValidAuthentication_ReturnsOk()
    {
        // Arrange
        var validJwtCookie = await _utility.LoginAndGetJwtCookieAsync("test.user@example.com", "Pass123!");
        
        using var getAllTagsRequest = new HttpRequestMessage(HttpMethod.Get, BaseUrl);
        getAllTagsRequest.Headers.Add("Cookie", validJwtCookie);

        // Act
        using var getAllTagsResponse = await _client.SendAsync(getAllTagsRequest, _cancellationToken);

        // Assert
        getAllTagsResponse.Should().Be200Ok()
            .And.BeAs(new[]
            {
                new { TagId = 1, Name = "Tools" },
                new { TagId = 2, Name = "Community" },
                new { TagId = 3, Name = "Git" }
            });
    }

    [Fact]
    public async Task RetrieveAllAsync_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        using var getAllTagsRequest = new HttpRequestMessage(HttpMethod.Get, BaseUrl);

        // Act
        using var getAllTagsResponse = await _client.SendAsync(getAllTagsRequest, _cancellationToken);

        // Assert
        getAllTagsResponse.Should().Be401Unauthorized();
    }
}