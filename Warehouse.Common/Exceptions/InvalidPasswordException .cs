using System;

namespace Warehouse.Common.Exceptions
{
  public class InvalidPasswordException : Exception
  {
    public InvalidPasswordException() : base() { }
    public InvalidPasswordException(string message) : base(message) { }
    public InvalidPasswordException(string message, Exception innerException) : base(message, innerException) { }
  }
}
