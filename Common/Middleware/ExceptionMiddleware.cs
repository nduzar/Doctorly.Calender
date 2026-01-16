using System.Net;
using System.Text.Json;
using Doctorly.Calendar.Core.Exceptions;

namespace Doctorly.Calendar.Common.Middleware;

/// <summary>
/// Senior Concept: Global Exception Handler.
/// Captures all unhandled exceptions and returns a consistent JSON structure.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Default to 500 Server Error
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var message = "An internal server error occurred.";

        // If it's our custom business error, return 400 Bad Request
        if (exception is DomainException)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            message = exception.Message;
        }

        context.Response.StatusCode = statusCode;

        var response = new
        {
            StatusCode = statusCode,
            Message = message,
            Detailed = exception.GetType().Name
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}