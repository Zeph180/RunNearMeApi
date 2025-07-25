using Microsoft.AspNetCore.Builder;

namespace Application.Exensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<global::Application.Middlewares.GlobalExceptionHandlingMiddleware>();
    }
}