using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace bookmark_manager_app.Exceptions.Handlers;

public sealed class GlobalExceptionHandler( ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", httpContext.TraceIdentifier);

        var (httpStatusCode, title) = MapException(exception);
        httpContext.Response.StatusCode = httpStatusCode;
        var problemDetails = new ProblemDetails
        {
            Status = httpStatusCode,
            Title = title,
            Type = GetProblemType(httpStatusCode),
            Instance = httpContext.Request.Path,
            Detail = GetSafeErrorMessage(exception, httpContext),
            Extensions =
            {
                ["traceId"] = httpContext.TraceIdentifier,
                ["timestamp"] = DateTime.UtcNow
            }
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }

    private static (int StatusCode, string Title) MapException(Exception exception) => exception switch
    {
        ApiException apiException => (apiException.HttpStatusCode, apiException.Message),
        ArgumentNullException => (StatusCodes.Status400BadRequest, "Invalid argument provided."),
        ArgumentException => (StatusCodes.Status400BadRequest, "Invalid argument provided."),
        UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized."),
        _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
    };

    private static string GetProblemType(int statusCode) => statusCode switch
    {
        400 => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
        401 => "https://tools.ietf.org/html/rfc9110#section-15.5.2",
        403 => "https://tools.ietf.org/html/rfc9110#section-15.5.4",
        404 => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
        409 => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
        _ => "https://tools.ietf.org/html/rfc9110#section-15.6.1"
    };

    private static string? GetSafeErrorMessage(Exception exception, HttpContext context)
    {
        // Only expose details in development
        var env = context.RequestServices.GetRequiredService<IHostEnvironment>();
        if (env.IsDevelopment())
        {
            return exception.Message;
        }

        // In production, only expose messages from our own exceptions
        return exception is ApiException ? exception.Message : null;
    }
}