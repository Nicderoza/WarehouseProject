using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Warehouse.Common.Security;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Data.Models;
using Microsoft.Extensions.Logging;
using Warehouse.Common.PasswordServices; // Assicurati che questo sia corretto

namespace Warehouse.Service.Services
{
  public class AuthenticationService
  {
    private readonly IUserRepository _userRepository;
    private readonly PasswordService _passwordService;
    private readonly JWT _jwtService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(IUserRepository userRepository, Common.PasswordServices.PasswordService passwordService, JWT jwtService, IConfiguration configuration, ILogger<AuthenticationService> logger)
    {
      _userRepository = userRepository;
      _passwordService = passwordService;
      _jwtService = jwtService;
      _configuration = configuration;
      _logger = logger;
    }

    public async Task<string> Authenticate(string email, string password)
    {
      _logger.LogInformation($"Authenticating user with email: {email}");
      var user = await _userRepository.GetByEmailAsync(email);

      if (user == null)
      {
        _logger.LogWarning($"User with email: {email} not found.");
        return null;
      }

      var verificationResult = _passwordService.VerifyPassword(password, user.PasswordHash);

      if (verificationResult == PasswordVerificationResult.Success)
      {
        var secretKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing from configuration.");
        var issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing from configuration.");
        var audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing from configuration.");

        string token = _jwtService.GenerateToken(user, secretKey, issuer, audience);
        _logger.LogInformation($"User with email: {email} authenticated successfully. Token generated.");
        return token;
      }
      else
      {
        _logger.LogWarning($"Invalid password for user with email: {email}");
        return null;
      }
    }

    public async Task<Users> GetUserByEmailAsync(string email)
    {
      _logger.LogInformation($"Getting user by email: {email}");
      var user = await _userRepository.GetByEmailAsync(email);
      if (user == null)
      {
        _logger.LogWarning($"User with email {email} not found");
      }
      return user;
    }

    public async Task<string> GenerateJwtTokenAsync(Users user)
    {
      var secretKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing from configuration.");
      var issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing from configuration.");
      var audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing from configuration.");
      _logger.LogInformation($"Generating JWT token for user: {user.Email}");
      return _jwtService.GenerateToken(user, secretKey, issuer, audience);
    }
  }
}
