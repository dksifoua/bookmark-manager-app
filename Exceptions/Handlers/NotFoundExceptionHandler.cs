using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Exceptions.Handlers;

public sealed class NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException notFound)
        {
            return false;
        }
        logger.LogWarning("Resource not found: {Message}", notFound.Message);
        httpContext.Response.StatusCode = notFound.HttpStatusCode;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = notFound.HttpStatusCode,
            Title = "Resource Not Found",
            Detail = notFound.Message
        }, cancellationToken);
        return true;
    }
}