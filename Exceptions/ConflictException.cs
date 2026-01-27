using System.Net;

namespace BookmarkManagerApp.Exceptions;

public class ConflictException(string message) : ApiException(message, HttpStatusCode.Conflict);