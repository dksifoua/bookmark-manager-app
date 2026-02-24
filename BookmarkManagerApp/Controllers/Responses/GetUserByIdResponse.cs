using BookmarkManagerApp.Models;

namespace BookmarkManagerApp.Controllers.Responses;

public record GetUserByIdResponse(string Fullname, string Email)
{
    public static GetUserByIdResponse FromModel(User user) => new(user.Fullname, user.Email);
}