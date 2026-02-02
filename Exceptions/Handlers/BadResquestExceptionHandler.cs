using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Exceptions.Handlers;

public sealed class HandlerBadRequestException(ILogger<HandlerBadRequestException> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BadRequestException badRequest)
        {
            return false;
        }
        logger.LogWarning("Bad request: {Message}", badRequest.Message);
        httpContext.Response.StatusCode = badRequest.HttpStatusCode;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = badRequest.HttpStatusCode,
            Title = "Bad Request",
            Detail = badRequest.Message
        }, cancellationToken);
        return true;
    }
}