using System.Text.Json;
using Application.Middlewares.ErrorHandling;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "An unhandled exception occured");
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            BusinessException bex => new ErrorResponse
            {
                Message = bex.Message,
                ErrorCode = bex.ErrorCode,
                StatusCode = bex.StatusCode,
                TraceId = context.TraceIdentifier,
                TimeStamp = DateTime.UtcNow
            },
            ValidationException vex => new ValidationErrorResponse
            {
                Message = vex.Message,
                ErrorCode = "VALIDATION_ERROR",
                StatusCode = 400,
                TraceId = context.TraceIdentifier,
                TimeStamp = DateTime.UtcNow.Date,
                ValidationErrors = vex.Errors
            },
            ArgumentNullException => new ErrorResponse
            {
                Message = "Invalid request parameter",
                ErrorCode = "INVALID_REQUEST_PARAMETERS",
                StatusCode = 400,
                TraceId = context.TraceIdentifier,
                TimeStamp = DateTime.UtcNow
            },
            UnauthorizedAccessException => new ErrorResponse
            {
                Message = "Unauthorized access",
                ErrorCode = "UNAUTHORIZED",
                StatusCode = 401,
                TraceId = context.TraceIdentifier,
                TimeStamp = DateTime.UtcNow
            },
            KeyNotFoundException => new ErrorResponse
            {
                Message = "Key not found",
                ErrorCode = "KEY_NOT_FOUND_ERROR",
                StatusCode = 404,
                TraceId = context.TraceIdentifier,
                TimeStamp = DateTime.UtcNow
            },
            TimeoutException => new ErrorResponse
            {
                Message = "Timeout",
                ErrorCode = "TIMEOUT",
                StatusCode = 408,
                TraceId = context.TraceIdentifier,
                TimeStamp = DateTime.UtcNow
            },
            _ => new ErrorResponse
            {
                Message = "Internal server error",
                ErrorCode = "INTERNAL_SERVER_ERROR",
                StatusCode = 500,
                TraceId = context.TraceIdentifier,
                TimeStamp = DateTime.UtcNow,
                Details = new Dictionary<string, object>
                {
                    { "ExceptionType", exception.GetType().Name }
                }
            }
        };

        response.StatusCode = errorResponse.StatusCode;
        
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        await response.WriteAsync(jsonResponse);
    }
}