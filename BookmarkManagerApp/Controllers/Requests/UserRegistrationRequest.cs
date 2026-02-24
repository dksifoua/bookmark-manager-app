namespace BookmarkManagerApp.Controllers.Requests;

public record UserRegistrationRequest(string Fullname, string Email, string Password);