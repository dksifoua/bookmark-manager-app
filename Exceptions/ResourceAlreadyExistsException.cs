using System.Net;

namespace BookmarkManagerApp.Exceptions;

public class ResourceAlreadyExistsException(string message) : ApiException(message, HttpStatusCode.BadRequest);