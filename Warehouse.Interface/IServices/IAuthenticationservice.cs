// Warehouse.Interfaces.IServices/IAuthenticationService.cs
using Microsoft.AspNetCore.Identity;
using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data.Models; // Assicurati di importare il namespace corretto

namespace Warehouse.Interfaces.IServices
{
  public interface IAuthenticationService
  {
    Task<BaseResponse<DTOLoginSuccessResponse>> LoginAsync(DTOLogin loginDTO); // <<< MODIFICATO QUI
    string HashPassword(Users user, string password);
    string HashNewPassword(string newPassword);
    PasswordVerificationResult VerifyPassword(Users user, string hashedPassword, string providedPassword);
  }
}
