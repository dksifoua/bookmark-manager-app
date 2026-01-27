using BookmarkManagerApp.Services.Commands;

namespace BookmarkManagerApp.Controllers.Requests;

public record UserLoginRequest(string Email, string Password)
{
    public LoginUserCommand ToCommand() => new (Email, Password);
}