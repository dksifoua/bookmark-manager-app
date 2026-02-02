namespace bookmark_manager_app.Exceptions;

public sealed class NotFoundException(string message) : ApiException(message, StatusCodes.Status404NotFound);