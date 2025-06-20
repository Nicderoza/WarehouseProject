using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Warehouse.Interfaces.IServices;
using Warehouse.Data.Models;

namespace Warehouse.Service.Services
{
  public class JWTService : IJWTService
  {
    private readonly IConfiguration _configuration;

    public JWTService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public string GenerateToken(Users user)
    {
      var jwtKey = _configuration["Jwt:Key"];
      var jwtIssuer = _configuration["Jwt:Issuer"];
      var jwtAudience = _configuration["Jwt:Audience"];
      var jwtExpirationHours = _configuration["Jwt:ExpirationHours"];

      if (string.IsNullOrEmpty(jwtKey)) throw new ArgumentNullException("Jwt:Key is not configured.");
      if (string.IsNullOrEmpty(jwtIssuer)) throw new ArgumentNullException("Jwt:Issuer is not configured.");
      if (string.IsNullOrEmpty(jwtAudience)) throw new ArgumentNullException("Jwt:Audience is not configured.");
      if (string.IsNullOrEmpty(jwtExpirationHours)) throw new ArgumentNullException("Jwt:ExpirationHours is not configured.");

      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Role, user.RoleID.ToString())
            };

      int expirationHours = Convert.ToInt32(jwtExpirationHours);


      DateTime currentTimeUtc = DateTime.UtcNow;

      DateTime calculatedExpirationTime = currentTimeUtc.AddHours(expirationHours);



      // Dichiarazione e inizializzazione della variabile 'token' 
      var token = new JwtSecurityToken(
          issuer: jwtIssuer,
          audience: jwtAudience,
          claims: claims,
          expires: calculatedExpirationTime, 
          signingCredentials: credentials);

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
