
using System.Text.Json;

namespace UniversityApi.Common;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
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
            _logger.LogError(
                ex,
                "Необработанная ошибка. TraceId: {TraceId}",
                context.TraceIdentifier);

            if (context.Response.HasStarted)
                throw;

            context.Response.Clear();
            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            context.Response.ContentType = "application/json";

            var response = ReturnResult<object>.Failure(
                500,
                "INTERNAL_SERVER_ERROR",
                "Произошла внутренняя ошибка сервера.",
                context.TraceIdentifier);

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}