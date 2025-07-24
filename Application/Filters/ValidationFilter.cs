using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ValidationException = Application.Middlewares.ErrorHandling.ValidationException;

namespace Application.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

            context.Result = new BadRequestObjectResult(new
            {
                message = "Validation failed",
                errorCode = "VALIDATION_ERROR",
                statusCode = 400,
                traceId = context.HttpContext.TraceIdentifier,
                timeStamp = DateTime.UtcNow,
                details = errors // 👈✅ this now shows error messages
            });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}