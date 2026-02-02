namespace bookmark_manager_app.Exceptions;

public sealed class CustomValidationException(IDictionary<string, string[]> errors)
    : ApiException("One or more validation errors occurred.", StatusCodes.Status400BadRequest)
{
    public IDictionary<string, string[]> Errors { get; } = errors;
}