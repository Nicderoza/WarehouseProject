namespace Warehouse.Common.Responses
{
  public class ErrorControl
  {
    public static string GetErrorMessage(ErrorType errorType)
    {
      switch (errorType)
      {
        case ErrorType.InvalidPassword:
          return "The password provided is invalid. Please try again.";
        case ErrorType.UserNotFound:
          return "The user was not found. Please check the provided credentials.";
        case ErrorType.InvalidEmail:
          return "The email provided is invalid. Please provide a valid email address.";
        case ErrorType.ProfileAlreadyExists:
          return "A profile with this information already exists.";
        default:
          return "An unknown error occurred. Please try again later.";
      }
    }
  }

  public enum ErrorType
  {
    InvalidPassword,
    UserNotFound,
    InvalidEmail,
    ProfileAlreadyExists,
    GeneralError
  }
}
