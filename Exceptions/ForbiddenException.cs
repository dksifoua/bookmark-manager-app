namespace bookmark_manager_app.Exceptions;

public sealed class ForbiddenException(string message) : ApiException(message, StatusCodes.Status403Forbidden);
