using BookmarkManagerApp.Services.Commands;

namespace BookmarkManagerApp.Controllers.Requests;

public record UserRegistrationRequest(string Fullname, string Email, string Password)
{
    public RegisterUserCommand ToCommand() => new(Fullname, Email, Password);
}