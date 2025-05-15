namespace Warehouse.Interfaces.IServices
{
  public interface IPasswordService
  {
    string HashPassword(string password);
    bool VerifyPassword(string password, string storedHash);
  }
}
