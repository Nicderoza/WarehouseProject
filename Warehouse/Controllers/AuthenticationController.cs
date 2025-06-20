using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Net;
using Warehouse.Common.DTOs;  
using Warehouse.Interfaces.IServices;

namespace Warehouse.Controllers
{
  [ApiController]
  [Route("api/[controller]")]   
  public class AuthenticationController : ControllerBase
  {
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserService _userService;
    private readonly IRegistrationService _registrationService;   

    public AuthenticationController(IAuthenticationService authenticationService,
                                    IUserService userService,
                                    IRegistrationService registrationService)   
    {
      _authenticationService = authenticationService;
      _userService = userService;
      _registrationService = registrationService;
    }

    [HttpPost("login")] 
    public async Task<IActionResult> Login([FromBody] DTOLogin loginDTO)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var response = await _authenticationService.LoginAsync(loginDTO);

      if (!response.Success)
      {
        if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
          return Unauthorized(response.Message);
        }
        if (response.StatusCode == (int)HttpStatusCode.BadRequest)
        {
          return BadRequest(response.Message);
        }
        return StatusCode(response.StatusCode ?? (int)HttpStatusCode.InternalServerError, response.Message);
      }

      // Se il login ha successo, potresti voler restituire non solo il token, ma anche i dati dell'utente (incluso il ruolo)
      // Questo è fondamentale per il frontend per sapere chi è l'utente e quale ruolo ha.
      // Il metodo LoginAsync nel tuo AuthenticationService dovrebbe restituire anche l'oggetto utente o un DTO contenente i dati dell'utente.
      // Esempio (presupponendo che response.Data sia il token e response.User sia l'oggetto utente):
      // return Ok(new { Token = response.Data, User = response.User });

      // Per ora, ci atteniamo a quanto già presente, ma tieni a mente la necessità di inviare i dati utente
      // per popolare il BehaviorSubject nel frontend.
      return Ok(new { Token = response.Data });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] DTOCreateUser userDto)
    {
      bool success = await _registrationService.RegisterUser(userDto);

      if (success)
      {
        return Ok(new { success = true, message = "User registered successfully." });
      }
      else
      {
        return BadRequest(new { success = false, message = "Registration failed." });
      }
    }



    [HttpPost("register-supplier")] 
    public async Task<IActionResult> RegisterSupplier([FromBody] DTORegisterSupplier supplierDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var success = await _registrationService.RegisterSupplier(supplierDto);

      if (!success)
      {
        return BadRequest("Supplier registration failed. Email or company might already be in use.");
      }

      return Ok("Supplier registered successfully.");
    }


    [Authorize]
    [HttpPost("change-password")] 
    public async Task<IActionResult> ChangePassword([FromBody] DTOChangePassword changePasswordDTO)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
      {
        return Unauthorized("User ID not found or invalid in token claims.");
      }

      var response = await _userService.ChangePasswordAsync(userId, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);

      if (!response.Success)
      {
        if (response.StatusCode == (int)HttpStatusCode.NotFound || response.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
          return Unauthorized(response.Message);
        }
        if (response.StatusCode == (int)HttpStatusCode.BadRequest)
        {
          return BadRequest(response.Message);
        }
        return StatusCode(response.StatusCode ?? (int)HttpStatusCode.InternalServerError, response.Message);
      }

      return Ok(response.Message);
    }
  }
}
