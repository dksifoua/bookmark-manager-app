namespace bookmark_manager_app.Controllers.Requests;

public record UserRegistrationRequest(string Fullname, string Email, string Password);