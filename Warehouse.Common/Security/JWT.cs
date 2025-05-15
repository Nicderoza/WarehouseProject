using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Warehouse.Data.Models;

namespace Warehouse.Common.Security
{
  public class JWT
  {
    // Metodo per generare il token JWT
    public string GenerateToken(Users user, string secretKey, string issuer, string audience)
    {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Email),

                // Aggiungi eventuali altri claims, ad esempio per i ruoli
                // new Claim(ClaimTypes.Role, user.Role)
            };

      var tokenDescriptor = new JwtSecurityToken(
          issuer: issuer,
          audience: audience,
          claims: claims,
          expires: DateTime.Now.AddHours(1), // Imposta la durata di scadenza del token
          signingCredentials: credentials
      );

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.WriteToken(tokenDescriptor);

      return token;
    }
  }
}
