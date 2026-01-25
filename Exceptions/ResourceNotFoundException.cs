using System.Net;

namespace BookmarkManagerApp.Exceptions;

public class ResourceNotFoundException(string message) : ApiException(message, HttpStatusCode.NotFound);