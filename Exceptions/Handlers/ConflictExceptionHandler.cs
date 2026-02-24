using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Exceptions.Handlers;

public sealed class ConflictExceptionHandler(ILogger<ConflictExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ConflictException conflict)
        {
            return false;
        }

        logger.LogWarning("Conflict detected: {Message}", conflict.Message);
        httpContext.Response.StatusCode = conflict.HttpStatusCode;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = conflict.HttpStatusCode,
            Title = "Conflict",
            Detail = conflict.Message
        }, cancellationToken);
        return true;
    }
}