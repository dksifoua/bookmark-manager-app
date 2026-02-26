using System.Net;

namespace BookmarkManagerApp.IntegrationTests;

public class BookmarkManagerAppIntegrationTest(BookmarkManagerAppFactory factory): IClassFixture<BookmarkManagerAppFactory>
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
}