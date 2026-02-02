namespace bookmark_manager_app.Exceptions;

public sealed class ConflictException (string message) : ApiException(message, StatusCodes.Status409Conflict);