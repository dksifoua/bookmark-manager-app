using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace BookmarkManagerApp.IntegrationTests.controllers;

public class Utility(HttpClient client, CancellationToken cancellationToken)
{
    private const string BaseUrl = "/api/auth";
    public async Task<string> LoginAndGetJwtCookieAsync(string email, string password)
    {
        using var loginRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/login");
        var loginRequestData = new
        {
            Email = email,
            Password = password
        };
        loginRequest.Content = JsonContent.Create(loginRequestData);

        using var loginResponse = await client.SendAsync(loginRequest, cancellationToken);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        return loginResponse.Headers.TryGetValues("Set-Cookie", out var values)
            ? values.First()
            : throw new InvalidOperationException("Login response did not contain Set-Cookie header.");
    }
}