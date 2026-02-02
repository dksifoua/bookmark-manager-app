using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Exceptions.Handlers;

public class UnauthorizedExceptionHandler(ILogger<ValidationExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not UnauthorizedException unauthorizedException)
        {
            return false;
        }

        logger.LogWarning("Unauthorized: {Message}", unauthorizedException.Message);

        httpContext.Response.StatusCode = unauthorizedException.HttpStatusCode;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = unauthorizedException.HttpStatusCode,
            Title = "Unauthorized",
            Detail = unauthorizedException.Message
        }, cancellationToken);

        return true;
    }
}