﻿namespace Application.Wrappers;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public List<string>? Errors { get; set; }


    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation successful", int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            StatusCode = statusCode
        };
    }
    
    public static ApiResponse<T> FailResponse(string message, int statusCode = 400, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            StatusCode = statusCode,
            Errors = errors
        };
    }
}