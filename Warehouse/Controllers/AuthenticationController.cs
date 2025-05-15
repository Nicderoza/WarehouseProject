using Microsoft.AspNetCore.Mvc;
using Warehouse.Service.Services; // Assuming your AuthenticationService is in this namespace
using Warehouse.Data.Models; // Assuming your Users model is in this namespace

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
  private readonly AuthenticationService _authenticationService;

  public AuthenticationController(AuthenticationService authenticationService)
  {
    _authenticationService = authenticationService;
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login(string email, string password)
  {
    var token = await _authenticationService.Authenticate(email, password);
    if (token == null) // Changed the condition to check for null token
    {
      return Unauthorized("Credenziali non valide."); // More generic error message
    }
    return Ok(new { Token = token });
  }

  // Example action that might use GenerateJwtTokenAsync
  [HttpPost("generateToken")]
  public async Task<IActionResult> GenerateToken(string email)
  {
    var user = await _authenticationService.GetUserByEmailAsync(email);
    if (user == null)
    {
      return NotFound("User not found.");
    }
    var token = await _authenticationService.GenerateJwtTokenAsync(user); // Line 37 is likely here or similar
    return Ok(new { Token = token });
  }
}
