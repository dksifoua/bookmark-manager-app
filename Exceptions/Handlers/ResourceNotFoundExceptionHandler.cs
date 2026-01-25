using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Exceptions.Handlers;

public class ResourceNotFoundExceptionHandler(ILogger<ValidationExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ResourceNotFoundException notFoundException)
        {
            return false;
        }

        logger.LogWarning("Resource Not Found: {Message}", notFoundException.Message);

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = 404,
            Title = "Resource Not Found",
            Detail = notFoundException.Message
        }, cancellationToken);

        return true;
    }
}