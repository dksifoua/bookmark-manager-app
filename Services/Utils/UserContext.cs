using System.Security.Claims;

namespace bookmark_manager_app.Services.Utils;

public class UserContext(ClaimsPrincipal principal)
{
    public long UserId { get; } = JwtClaimRetriever.TryGetUserIdFromClaimsPrincipalOrElseThrow(principal);
}