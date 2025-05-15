using Microsoft.AspNetCore.Mvc;
using Warehouse.Common.DTOs;
using Warehouse.Interfaces.IServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Warehouse.Common.Security;

namespace Warehouse.WEB.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Produces("application/json")]
  public class UsersController : ControllerBase
  {
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpGet]
    [Authorize(Policy = Policies.User)]
    [Route("GetUserData")]
    public IActionResult GetUserData()
    {
      return Ok("Questi sono dati per un utente normale");
    }

    [HttpGet]
    [Authorize(Policy = Policies.Admin)]
    [Route("GetAdminData")]
    public IActionResult GetAdminData()
    {
      return Ok("Questi sono dati per un utente amministratore");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
      var response = await _userService.GetAllAsync();
      if (response.Success)
      {
        return Ok(response.Data);
      }
      else
      {
        return BadRequest(response.Message);
      }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
      var response = await _userService.GetByIdAsync(id);
      if (response.Success)
      {
        return Ok(response.Data);
      }
      else if (response.NotFound)
      {
        return NotFound(response.Message);
      }
      else
      {
        return BadRequest(response.Message);
      }
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] DTOCreateUser createUserDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var createdUserDto = await _userService.CreateAsync(createUserDto);

      // Qui potresti voler restituire un CreatedAtActionResult (201) con l'utente creato
      return Ok(createdUserDto);
    }
  }
}
