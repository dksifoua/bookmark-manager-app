namespace BookmarkManagerApp.Exceptions;

public sealed class BadRequestException(string message) : ApiException(message, StatusCodes.Status400BadRequest);