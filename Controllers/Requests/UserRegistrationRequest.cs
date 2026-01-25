using System.Net.Mail;
using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Services.Commands;

namespace BookmarkManagerApp.Controllers.Requests;

public record UserRegistrationRequest(string FullName, string Email, string Password, string ConfirmedPassword)
{
    public RegisterUserCommand ToCommand() => new(FullName, Email, Password);
}