using System.Net;

namespace BookmarkManagerApp.Exceptions;

public sealed class ValidationException(IDictionary<string, string[]> errors)
    : ApiException("One or more validation errors occurred.", HttpStatusCode.BadRequest)
{
    public IDictionary<string, string[]> Errors { get; } = errors;
}