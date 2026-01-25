using System.Net;

namespace BookmarkManagerApp.Exceptions;

public class BadRequestException(string message) : ApiException(message, HttpStatusCode.BadRequest);