using Domain.Models;

namespace Application.Middlewares.ErrorHandling;

public class ValidationErrorResponse : ErrorResponse
{
    public Dictionary<string, string[]>? ValidationErrors { get; set; }
}