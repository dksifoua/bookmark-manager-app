using System.Security.Claims;

namespace BookmarkManagerApp.Services.Utils;

public class UserContext(ClaimsPrincipal principal)
{
    public long UserId { get; } = JwtClaimRetriever.TryGetUserIdFromClaimsPrincipalOrElseThrow(principal);
}