using Warehouse.Data.Models; // Assicurati che il namespace sia corretto

namespace Warehouse.Interfaces.IServices
{
  public interface IJWTService
  {
    string GenerateToken(Users user); // Per ora, concentriamoci sulle info base dell'utente
  }
}
