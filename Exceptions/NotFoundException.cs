namespace BookmarkManagerApp.Exceptions;

public sealed class NotFoundException(string message) : ApiException(message, StatusCodes.Status404NotFound);