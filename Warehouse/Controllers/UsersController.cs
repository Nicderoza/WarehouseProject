using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Common.DTOs.DTOAdmin;
using Warehouse.Interfaces.IServices;

namespace Warehouse.WEB.Controllers
{
  [Route("api/[users]")]
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
      else if (response.StatusCode == (int)System.Net.HttpStatusCode.NotFound)
      {
        return NotFound(response.Message);
      }
      else
      {
        return BadRequest(response.Message);
      }
    }

    [HttpPost]

    public async Task<IActionResult> CreateUser([FromBody] DTOCreateUser createUserDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      var createdUserResponse = await _userService.CreateAsync(createUserDto);
      if (createdUserResponse.Success)
      {
        return Ok(createdUserResponse.Data);
      }
      else
      {
        return BadRequest(createdUserResponse.Message);
      }
    }
    [HttpPut("{userId}/toggle-status")]

    public async Task<IActionResult> ToggleStatus(int userId)
    {
      var result = await _userService.ToggleStatusAsync(userId);
      return Ok(result);
    }

    [HttpPost("associate")]

    public async Task<IActionResult> AssociateSupplier([FromBody] DTOAssociation dto)
    {
      await _userService.AssociateSupplierAsync(dto.UserID, dto.SupplierID);
      return Ok();
    }

    [HttpPost("dissociate")]
    public async Task<IActionResult> DissociateSupplier([FromBody] DTODissociation dto)
    {
      await _userService.DissociateSupplierAsync(dto.UserID, dto.SupplierID); 
      return Ok();
    }





    [HttpPost("change-role")]

    public async Task<IActionResult> ChangeRole([FromBody] DTOChangeRole dto)
    {
      await _userService.ChangeRoleAsync(dto.UserID, dto.NewRole);
      return Ok();
    }

    [HttpGet("by-user/{userId}")]
    public async Task<IActionResult> GetUser(int userId)
    {
      var user = await _userService.GetByIdAsync(userId);
      if (user == null) return NotFound();
      return Ok(user);
    }

    [HttpGet("without-supplier")]
    public async Task<IActionResult> GetUsersWithoutSupplier()
    {
      var users = await _userService.GetUsersWithoutSupplierAsync();
      return Ok(users);
    }

    [HttpGet("with-supplier")]
    public async Task<IActionResult> GetUsersWithSupplier()
    {
      var users = await _userService.GetUsersWithSuppliersAsync();
      return Ok(users);
    }
    [HttpGet("by-supplier/{supplierId}")]
    public async Task<IActionResult> GetUsersBySupplier(int supplierId)
    {
      var users = await _userService.GetUsersBySupplierAsync(supplierId);
      return Ok(users);
    }

  }
}
