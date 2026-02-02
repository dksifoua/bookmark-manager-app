using bookmark_manager_app.Models;

namespace bookmark_manager_app.Controllers.Responses;

public record GetUserByIdResponse(string Fullname, string Email)
{
    public static GetUserByIdResponse FromModel(User user) => new(user.Fullname, user.Email);
}