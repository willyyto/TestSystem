using TestSystem.Core.Dtos;

namespace TestSystem.Extensions;

public static class ApiResponseExtensions
{
    /// <summary>
    /// Creates a successful API response
    /// </summary>
    public static ApiResponseDto<T> ToSuccessResponse<T>(this T data, string? message = null)
    {
        return new ApiResponseDto<T>(
            Success: true,
            Data: data,
            Message: message ?? "Operation completed successfully",
            Errors: null,
            StatusCode: 200
        );
    }

    /// <summary>
    /// Creates an error API response from a string message
    /// </summary>
    public static ApiResponseDto<T> ToErrorResponse<T>(this string message, List<string>? errors = null, int statusCode = 400)
    {
        return new ApiResponseDto<T>(
            Success: false,
            Data: default(T),
            Message: message,
            Errors: errors,
            StatusCode: statusCode
        );
    }

    /// <summary>
    /// Creates an error API response from an exception
    /// </summary>
    public static ApiResponseDto<T> ToErrorResponse<T>(this Exception exception, int statusCode = 500)
    {
        var errors = new List<string> { exception.Message };
        
        // Add inner exception messages if they exist
        var innerException = exception.InnerException;
        while (innerException != null)
        {
            errors.Add(innerException.Message);
            innerException = innerException.InnerException;
        }

        return new ApiResponseDto<T>(
            Success: false,
            Data: default(T),
            Message: "An error occurred while processing the request",
            Errors: errors,
            StatusCode: statusCode
        );
    }

    /// <summary>
    /// Creates an error API response from validation errors
    /// </summary>
    public static ApiResponseDto<T> ToValidationErrorResponse<T>(this List<ValidationErrorDto> validationErrors)
    {
        var errorMessages = validationErrors.Select(e => $"{e.Field}: {e.Message}").ToList();
        
        return new ApiResponseDto<T>(
            Success: false,
            Data: default(T),
            Message: "Validation failed",
            Errors: errorMessages,
            StatusCode: 400
        );
    }

    /// <summary>
    /// Creates a not found error response
    /// </summary>
    public static ApiResponseDto<T> ToNotFoundResponse<T>(string? message = null)
    {
        return new ApiResponseDto<T>(
            Success: false,
            Data: default(T),
            Message: message ?? "Resource not found",
            Errors: null,
            StatusCode: 404
        );
    }

    /// <summary>
    /// Creates an unauthorized error response
    /// </summary>
    public static ApiResponseDto<T> ToUnauthorizedResponse<T>(string? message = null)
    {
        return new ApiResponseDto<T>(
            Success: false,
            Data: default(T),
            Message: message ?? "Unauthorized access",
            Errors: null,
            StatusCode: 401
        );
    }

    /// <summary>
    /// Creates a forbidden error response
    /// </summary>
    public static ApiResponseDto<T> ToForbiddenResponse<T>(string? message = null)
    {
        return new ApiResponseDto<T>(
            Success: false,
            Data: default(T),
            Message: message ?? "Access forbidden",
            Errors: null,
            StatusCode: 403
        );
    }

    /// <summary>
    /// Creates a created response (201)
    /// </summary>
    public static ApiResponseDto<T> ToCreatedResponse<T>(this T data, string? message = null)
    {
        return new ApiResponseDto<T>(
            Success: true,
            Data: data,
            Message: message ?? "Resource created successfully",
            Errors: null,
            StatusCode: 201
        );
    }

    /// <summary>
    /// Creates a no content response (204)
    /// </summary>
    public static ApiResponseDto<T> ToNoContentResponse<T>(string? message = null)
    {
        return new ApiResponseDto<T>(
            Success: true,
            Data: default(T),
            Message: message ?? "Operation completed successfully",
            Errors: null,
            StatusCode: 204
        );
    }
}