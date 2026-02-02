using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace bookmark_manager_app.Services.Utils;

public static class JwtClaimRetriever
{
    public static long TryGetUserIdFromClaimsPrincipalOrElseThrow(ClaimsPrincipal claimsPrincipal)
    {
        var jwtSubClaim = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        return long.TryParse(jwtSubClaim, out var userIdParsed) 
            ? userIdParsed 
            : throw new UnauthorizedAccessException("Invalid user ID in JWT token");
    }
}