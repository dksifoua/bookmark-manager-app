namespace BookmarkManagerApp.Services.Commands;

public record RegisterUserCommand(string FullName, string Email, string Password);