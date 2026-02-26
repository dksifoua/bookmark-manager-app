namespace BookmarkManagerApp.Exceptions;

public abstract class ApiException(string message, int httpStatusCode) : Exception(message)
{
    public int HttpStatusCode { get; } = httpStatusCode;
}

