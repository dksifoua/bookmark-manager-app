namespace bookmark_manager_app.Exceptions;

public sealed class BadRequestException(string message) : ApiException(message, StatusCodes.Status400BadRequest);