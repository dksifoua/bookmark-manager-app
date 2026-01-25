using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManagerApp.Exceptions.Handlers;

public sealed class ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException badRequestException)
        {
            return false;
        }

        logger.LogWarning("Validation failed: {Message}", badRequestException.Message);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails
        {
            Status = 400,
            Title = "Validation Failed",
            Errors = badRequestException.Errors
        }, cancellationToken);

        return true;
    }
}