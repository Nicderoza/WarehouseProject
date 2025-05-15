using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Warehouse.Common.PasswordServices
{
  public class PasswordService
  {
    private readonly IPasswordHasher<Users> _passwordHasher;
    private readonly ILogger<PasswordService> _logger;

    public PasswordService(IPasswordHasher<Users> passwordHasher, ILogger<PasswordService> logger)
    {
      _passwordHasher = passwordHasher;
      _logger = logger;
    }

    public string HashPassword(string password)
    {
      _logger.LogInformation("Hashing password");
      return _passwordHasher.HashPassword(default(Users), password);
    }

    public PasswordVerificationResult VerifyPassword(string password, string storedHash)
    {
      _logger.LogInformation("Verifying password");
      return _passwordHasher.VerifyHashedPassword(default(Users), storedHash, password);
    }
  }
}
