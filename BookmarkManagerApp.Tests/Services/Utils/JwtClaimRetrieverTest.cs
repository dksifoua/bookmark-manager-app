using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookmarkManagerApp.Services.Utils;

namespace BookmarkManagerApp.UnitTests.Services.Utils;

public class JwtClaimRetrieverTest
{
    [Fact]
    public void TryGetUserIdFromClaimsPrincipalOrElseThrow_ValidUserId_ReturnsUserId()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, "12345")
        };
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        // Act
        var result = JwtClaimRetriever.TryGetUserIdFromClaimsPrincipalOrElseThrow(claimsPrincipal);

        // Assert
        Assert.Equal(12345, result);
    }

    [Fact]
    public void TryGetUserIdFromClaimsPrincipalOrElseThrow_InvalidUserId_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, "invalid")
        };
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() =>
            JwtClaimRetriever.TryGetUserIdFromClaimsPrincipalOrElseThrow(claimsPrincipal));
    }

    [Fact]
    public void TryGetUserIdFromClaimsPrincipalOrElseThrow_MissingSubClaim_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var identity = new ClaimsIdentity();
        var claimsPrincipal = new ClaimsPrincipal(identity);

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() =>
            JwtClaimRetriever.TryGetUserIdFromClaimsPrincipalOrElseThrow(claimsPrincipal));
    }
}