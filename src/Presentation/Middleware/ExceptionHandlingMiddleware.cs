using MenuAdminAPI.Application.DTOs;
using System.Net;

namespace MenuAdminAPI.Presentation.Middleware;

/// <summary>
/// Middleware para tratamento centralizado de exceções
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "Exceção não tratada");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse(
            StatusCodes.Status500InternalServerError,
            "Erro interno do servidor",
            exception.Message
        );

        switch (exception)
        {
            case ArgumentException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = new ErrorResponse(
                    StatusCodes.Status400BadRequest,
                    "Argumento inválido",
                    exception.Message
                );
                break;

            case KeyNotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response = new ErrorResponse(
                    StatusCodes.Status404NotFound,
                    "Recurso não encontrado",
                    exception.Message
                );
                break;

            case InvalidOperationException:
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                response = new ErrorResponse(
                    StatusCodes.Status409Conflict,
                    "Operação inválida",
                    exception.Message
                );
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response = new ErrorResponse(
                    StatusCodes.Status500InternalServerError,
                    "Erro interno do servidor",
                    exception.Message
                );
                break;
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}

/// <summary>
/// Extensão para registrar o middleware
/// </summary>
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
