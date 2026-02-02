using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Exceptions.Handlers;

public sealed class ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not CustomValidationException validationException)
        {
            return false;
        }

        logger.LogWarning("Validation Error: {Message}", validationException.Message);

        httpContext.Response.StatusCode = validationException.HttpStatusCode;
        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails
        {
            Status = validationException.HttpStatusCode,
            Title = "Validation Error",
            Errors= validationException.Errors
        }, cancellationToken);

        return true;
    }
}