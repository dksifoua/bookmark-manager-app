using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Services.Commands;

namespace BookmarkManagerApp.Controllers.Requests;

public record RegisterDataRequest(string FullName, string Email, string Password, string ConfirmedPassword)
{
    public void Validate()
    {
        var errors = new Dictionary<string, string[]>();
        
        if (string.IsNullOrWhiteSpace(FullName)) errors.AddError("FullName", "Fullname is required!");

        if (string.IsNullOrWhiteSpace(Email)) errors.AddError("Email", "Email is required!");

        if (string.IsNullOrWhiteSpace(Password)) errors.AddError("Password", "Password is required!");

        if (string.IsNullOrWhiteSpace(ConfirmedPassword))
            errors.AddError("ConfirmedPassword", "ConfirmedPassword is required!");

        if (Password != ConfirmedPassword) errors.AddError("Password", "Passwords do not match!");

        if (errors.Count > 0) throw new ValidationException(errors);
    }
    
    public RegisterUserCommand ToCommand() => new(FullName, Email, Password);
}

public static class ValidationErrorsExtensions
{
    public static void AddError(this IDictionary<string, string[]> errors, string key, string message)
    {
        if (errors.TryGetValue(key, out var existing))
        {
            errors[key] = [.. existing, message];
        }
        else
        {
            errors[key] = [message];
        }
    }
}