using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data.Interfaces;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IServices;

namespace Warehouse.WEB.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class SuppliersController : ControllerBase
  {
    private readonly ISupplierService _supplierService;
    private readonly IUsersSuppliersRepository _usersSuppliersRepository;

    public SuppliersController(
        ISupplierService supplierService,
        IUsersSuppliersRepository usersSuppliersRepository)
    {
      _supplierService = supplierService;
      _usersSuppliersRepository = usersSuppliersRepository;
    }

    // GET: api/suppliers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DTOSupplier>>> GetSuppliers()
    {
      var response = await _supplierService.GetAllAsync();
      if (response.Success)
      {
        return Ok(response.Data);
      }
      else
      {
        return BadRequest(ErrorControl.GetErrorMessage(ErrorType.GeneralError));
      }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DTOSupplier>> GetSupplierById(int id)
    {
      var response = await _supplierService.GetByIdAsync(id);
      if (response.Success)
      {
        return Ok(response.Data);
      }
      else if (response.NotFound)
      {
        return NotFound(ErrorControl.GetErrorMessage(ErrorType.UserNotFound));
      }
      else
      {
        return BadRequest(ErrorControl.GetErrorMessage(ErrorType.GeneralError));
      }
    }

    [HttpGet("ByCity/{cityId}")]
    public async Task<ActionResult<IEnumerable<DTOSupplier>>> GetSuppliersByCity(int cityId)
    {
      var response = await _supplierService.GetSuppliersByCityAsync(cityId);
      if (response.Success)
      {
        return Ok(response.Data);
      }
      else
      {
        return BadRequest(ErrorControl.GetErrorMessage(ErrorType.GeneralError));
      }
    }

    [HttpPost]
    public async Task<ActionResult<DTOSupplier>> CreateSupplier([FromBody] DTOSupplier supplierDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var response = await _supplierService.AddAsync(supplierDto);
      if (response.Success)
      {
        return CreatedAtAction(nameof(GetSupplierById), new { id = response.Data.SupplierID }, response.Data);
      }
      else
      {
        return BadRequest(ErrorControl.GetErrorMessage(ErrorType.GeneralError));
      }
    }

    // PUT: api/suppliers/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSupplier(int id, [FromBody] DTOSupplier supplierDto)
    {
      if (!ModelState.IsValid || id != supplierDto.SupplierID)
      {
        return BadRequest(ModelState);
      }

      var existingSupplierResponse = await _supplierService.GetByIdAsync(id);
      if (!existingSupplierResponse.Success)
      {
        return NotFound(existingSupplierResponse.Message ?? ErrorControl.GetErrorMessage(ErrorType.UserNotFound));
      }

      var response = await _supplierService.UpdateAsync(id, supplierDto);
      if (response.Success)
      {
        return NoContent();
      }
      else if (response.NotFound)
      {
        return NotFound(response.Message);
      }
      else
      {
        return BadRequest(ErrorControl.GetErrorMessage(ErrorType.GeneralError));
      }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
      var existingSupplierResponse = await _supplierService.GetByIdAsync(id);
      if (!existingSupplierResponse.Success)
      {
        return NotFound(ErrorControl.GetErrorMessage(ErrorType.UserNotFound));
      }

      var response = await _supplierService.DeleteAsync(id);
      if (response.Success)
      {
        return NoContent();
      }
      else
      {
        return BadRequest(ErrorControl.GetErrorMessage(ErrorType.GeneralError));
      }
    }
    [HttpPost("register-or-use")]
    public async Task<ActionResult<BaseResponse<DTOSupplier>>> RegisterOrUseSupplier([FromBody] DTOSupplier dto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var existing = await _supplierService.GetByNameAsync(dto.CompanyName);
      if (existing != null)
      {
        return Ok(new BaseResponse<DTOSupplier>
        {
          Success = true,
          Message = "Fornitore già esistente, riutilizzato.",
          Data = existing
        });
      }
      var created = await _supplierService.AddAsync(dto);
      return CreatedAtAction(nameof(GetSupplierById), new { id = created.Data.SupplierID }, created);
    }
    [HttpDelete("associate")]
    public async Task<IActionResult> DeleteUserSupplierAssociation([FromQuery] int userId, [FromQuery] int supplierId)
    {
      await _usersSuppliersRepository.DeleteUsersSuppliersAsync(userId, supplierId);
      return Ok("Associazione eliminata.");
    }
    [HttpPost("associate")]
    public async Task<IActionResult> AssociateUserToSupplier([FromBody] DTOSupplierUser dto)
    {
      var existing = await _usersSuppliersRepository
          .GetUsersSuppliersByIdAsync(dto.UserID, dto.SupplierID);

      if (existing != null)
        return Conflict("Associazione già esistente.");

      var newLink = new UsersSuppliers
      {
        UserID = dto.UserID,
        SupplierID = dto.SupplierID
      };

      await _usersSuppliersRepository.AddUsersSuppliersAsync(newLink);
      return Ok("Associazione creata con successo.");
    }


    [HttpGet("by-user/{userId}/id")]
    public async Task<IActionResult> GetSupplierIdByUserId(int userId)
    {
      var association = await _usersSuppliersRepository.GetUsersSuppliersByUserIdAsync(userId);

      if (association == null)
        return NotFound("Associazione utente-fornitore non trovata.");

      return Ok(association.SupplierID);
    }

    [HttpGet("by-user/{userId}")]
    public async Task<IActionResult> GetSupplierByUserId(int userId)
    {
      var supplier = await _supplierService.GetSupplierByUserIdAsync(userId);
      if (supplier == null)
        return NotFound();
      return Ok(supplier);
    }

   }
 }

