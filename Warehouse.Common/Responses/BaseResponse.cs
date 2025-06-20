using System; // Make sure this is present

namespace Warehouse.Common.Responses
{
  public class BaseResponse<T>
  {
    public bool Success { get; set; }
    public bool NotFound { get; set; }
    public string Message { get; set; } = string.Empty; // Inizializza qui per evitare errori
    public T? Data { get; set; } // Rendi la proprietà Data nullable
    public ErrorControl? ErrorControl { get; set; } // Rendi la proprietà ErrorControl nullable
    public int? StatusCode { get; set; }

    public BaseResponse()
    {

    }

    // Rendi 'message' nullable in tutti i metodi statici
    public static BaseResponse<T> SuccessResponse(T data, string? message = null, int statusCode = 200)
    {
      return new BaseResponse<T>
      {
        Success = true,
        NotFound = false,
        Message = message ?? string.Empty, // Assicurati che non sia null
        Data = data,
        ErrorControl = null,
        StatusCode = statusCode
      };
    }

    // Rendi 'message' nullable
    public static BaseResponse<T> ErrorResponse(string? message, int? statusCode)
    {
      return new BaseResponse<T>
      {
        Success = false,
        NotFound = false,
        Message = message ?? "An unexpected error occurred.", // Messaggio di default se null
        Data = default(T), // default(T) è null per i tipi riferimento
        ErrorControl = null,
        StatusCode = statusCode ?? 500
      };
    }

    // Rendi 'message' nullable e aggiungi un default
    public static BaseResponse<T> NotFoundResponse(string? message = null, int? statusCode = 404)
    {
      return new BaseResponse<T>
      {
        Success = false,
        NotFound = true,
        Message = message ?? "Not Found", // Messaggio di default se null
        Data = default(T),
        ErrorControl = null,
        StatusCode = statusCode
      };
    }

    // Rendi 'message' nullable e aggiungi un default
    public static BaseResponse<T> InvalidEmail(string? message = null, int? statusCode = 404)
    {
      return new BaseResponse<T>
      {
        Success = false,
        NotFound = true,
        Message = message ?? "Email non valida.", // Messaggio di default se null
        Data = default(T),
        ErrorControl = null,
        StatusCode = statusCode
      };
    }

    // Rendi 'message' nullable e aggiungi un default
    public static BaseResponse<T> InvalidPassword(string? message = null, int? statusCode = 401)
    {
      return new BaseResponse<T>
      {
        Success = false,
        NotFound = false,
        Message = message ?? "Password non valida.", // Messaggio di default se null
        Data = default(T),
        ErrorControl = null,
        StatusCode = statusCode
      };
    }

    // Aggiungo solo i due metodi che usavi nel SupplierService:
    // BadRequestResponse e ConflictResponse.
    // Questi sono essenziali per il SupplierService, se non li hai già altrove.
    public static BaseResponse<T> BadRequestResponse(string? message = null, int? statusCode = 400)
    {
      return new BaseResponse<T>
      {
        Success = false,
        NotFound = false,
        Message = message ?? "Bad Request.",
        Data = default(T),
        ErrorControl = null,
        StatusCode = statusCode
      };
    }

    public static BaseResponse<T> ConflictResponse(string? message = null, int? statusCode = 409)
    {
      return new BaseResponse<T>
      {
        Success = false,
        NotFound = false,
        Message = message ?? "Conflict.",
        Data = default(T),
        ErrorControl = null,
        StatusCode = statusCode
      };
    }

    public bool Any()
    {
      throw new NotImplementedException();
    }
  }
}
  // Assicurati che ErrorControl e ErrorType siano in questo stesso namespace o uno accessibile.
  // Ho copiato la tua definizione di ErrorControl ed ErrorType da quella che hai pass
