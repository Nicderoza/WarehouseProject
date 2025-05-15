using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Common.DTOs;
using Warehouse.Service.Services;
using Microsoft.AspNetCore.Identity;
using Warehouse.Common.PasswordServices;

namespace Warehouse.WEB.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class PasswordController : ControllerBase
  {
    private readonly UserService _userService;
    private readonly PasswordService _passwordService;

    public PasswordController(UserService userService, PasswordService passwordService)
    {
      _userService = userService;
      _passwordService = passwordService;
    }

    [HttpPost("change")]
    public async Task<IActionResult> ChangePassword([FromBody] DTOChangePassword changePasswordDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
      if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
      {
        return Unauthorized();
      }

      var userResponse = await _userService.GetByIdAsync(userId);
      if (!userResponse.Success)
      {
        return NotFound(userResponse.Message);
      }
      var user = userResponse.Data;

      // Use the PasswordService correctly
      PasswordVerificationResult verificationResult = _passwordService.VerifyPassword(changePasswordDto.CurrentPassword, user.PasswordHash);
      if (verificationResult != PasswordVerificationResult.Success)
      {
        return BadRequest(new { Message = "La password attuale non è corretta." });
      }

      // Hash the new password, providing the password from the DTO
      var newPasswordHash = _passwordService.HashPassword(changePasswordDto.NewPassword);
      user.PasswordHash = newPasswordHash;

      var updateResult = await _userService.UpdateAsync(userId, new DTOUser
      {
        UserID = userId, // Utilizza UserID
        PasswordHash = newPasswordHash,
      });

      if (updateResult.Success)
      {
        return Ok(new { Message = "Password cambiata con successo." });
      }
      else
      {
        return BadRequest(new { Message = "Impossibile cambiare la password." });
      }
    }
  }
}
