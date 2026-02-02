namespace bookmark_manager_app.Exceptions;

public class UnauthorizedException(string message) : ApiException(message, StatusCodes.Status401Unauthorized);