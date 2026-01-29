using BookmarkManagerApp.Models;

namespace BookmarkManagerApp.Controllers.Responses;

public record GetCurrentUserResponse(string Fullname, string Email)
{
    public static GetCurrentUserResponse FromModel(User user) => new(user.Fullname, user.Email);
}