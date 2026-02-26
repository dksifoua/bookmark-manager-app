using System.Net;
using FluentAssertions;

namespace BookmarkManagerApp.IntegrationTests;

public class BookmarkManagerAppIntegrationTest(BookmarkManagerAppFactory factory)
    : IClassFixture<BookmarkManagerAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    [Fact]
    public async Task HealthCheck_ReturnsHealthyStatus()
    {
        var response = await _client.GetAsync("/api/health", _cancellationToken);
        Assert.Equal("Healthy", await response.Content.ReadAsStringAsync(_cancellationToken));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task RootEndpoint_Returns200_BadgeStype()
    {
        var response = await _client.GetAsync("/", _cancellationToken);
        response.Should().Be200Ok()
            .And.BeAs(new
            {
                schemaVersion = 1,
                label = "Render",
                message = "live",
                color = "green",
                namedLogo = "render"
            });
    }
}