namespace Application.Middlewares.ErrorHandling;

public class BusinessException : Exception
{
    public string ErrorCode { get; }
    public string StatusCode { get; }

    public BusinessException(string message, string errorCode = "BUSINESS_ERROR", string statusCode = "400") : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
}