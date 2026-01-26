using System.Net;

namespace BookmarkManagerApp.Exceptions;

public class UnauthorizedException(string message) : ApiException(message, HttpStatusCode.Unauthorized);