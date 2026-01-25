using System.Net;

namespace BookmarkManagerApp.Exceptions;

public abstract class ApiException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
    : Exception(message)
{
    public HttpStatusCode HttpStatusCode { get; } = httpStatusCode;
}