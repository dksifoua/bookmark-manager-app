using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Exceptions.Handlers;

public sealed class ForbiddenExceptionHandler(ILogger<ForbiddenExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ForbiddenException forbid)
        {
            return false;
        }
        logger.LogWarning("Forbidden: {Message}", forbid.Message);
        httpContext.Response.StatusCode = forbid.HttpStatusCode;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = forbid.HttpStatusCode,
            Type = "Forbid",
            Detail = forbid.Message
        }, cancellationToken);

        return true;
    }
}