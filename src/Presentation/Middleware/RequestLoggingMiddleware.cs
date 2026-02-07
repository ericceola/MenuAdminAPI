using System.Diagnostics;

namespace MenuAdminAPI.Presentation.Middleware;

/// <summary>
/// Middleware para logging de requisições HTTP
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var originalBodyStream = context.Response.Body;

        try
        {
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                _logger.LogInformation(
                    "Requisição iniciada: {Method} {Path} - IP: {RemoteIP}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Connection.RemoteIpAddress
                );

                await _next(context);

                stopwatch.Stop();

                var statusCode = context.Response.StatusCode;
                var duration = stopwatch.ElapsedMilliseconds;

                if (statusCode >= 400)
                {
                    _logger.LogWarning(
                        "Requisição concluída com erro: {Method} {Path} - Status: {StatusCode} - Duração: {Duration}ms",
                        context.Request.Method,
                        context.Request.Path,
                        statusCode,
                        duration
                    );
                }
                else
                {
                    _logger.LogInformation(
                        "Requisição concluída: {Method} {Path} - Status: {StatusCode} - Duração: {Duration}ms",
                        context.Request.Method,
                        context.Request.Path,
                        statusCode,
                        duration
                    );
                }

                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }
}

/// <summary>
/// Extensão para registrar o middleware
/// </summary>
public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}
