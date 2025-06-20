// Warehouse.Common.DTOs/DTOLoginSuccessResponse.cs
using Warehouse.Common.DTOs; // Assicurati di importare il namespace corretto per DTOUser

namespace Warehouse.Common.DTOs
{
  public class DTOLoginSuccessResponse
  {
    public string Token { get; set; }
    public DTOUser User { get; set; } // O un DTO più specifico per l'utente loggato se preferisci
  }
}
