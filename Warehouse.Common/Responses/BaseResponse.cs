using System;
namespace Warehouse.Common.Responses
{
  public class BaseResponse<T>
  {
    public bool Success { get; set; }
    public bool NotFound { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public ErrorControl ErrorControl { get; set; }
    public int? StatusCode { get; set; }

    public BaseResponse()
    {
    }

    public static BaseResponse<T> SuccessResponse(T data, string message = null, int statusCode = 200) // Added statusCode
    {
      return new BaseResponse<T>
      {
        Success = true,
        NotFound = false,
        Message = message,
        Data = data,
        ErrorControl = null,
        StatusCode = statusCode
      };
    }

    public static BaseResponse<T> ErrorResponse(string message, int? statusCode) // Changed parameters
    {
      return new BaseResponse<T>
      {
        Success = false,
        NotFound = false,
        Message = message,
        Data = default,
        ErrorControl = null, // Important: Set to null or a valid ErrorControl object
        StatusCode = statusCode ?? 500 // Provide a default status code
      };
    }

    public static BaseResponse<T> NotFoundResponse(string message = "Not Found", int? statusCode = 404) // Added statusCode
    {
      return new BaseResponse<T>
      {
        Success = false,
        NotFound = true,
        Message = message,
        Data = default,
        ErrorControl = null, // Important: Set to null or a valid ErrorControl object
        StatusCode = statusCode
      };
    }


    public bool Any()
    {
      throw new NotImplementedException();
    }
  }
}
