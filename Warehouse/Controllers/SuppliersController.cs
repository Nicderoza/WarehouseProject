using Microsoft.AspNetCore.Mvc;
using Warehouse.Common.DTOs;
using Warehouse.Interfaces.IServices;
using Warehouse.Common.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Warehouse.WEB.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class SuppliersController : ControllerBase
  {
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
      _supplierService = supplierService;
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

    // GET: api/suppliers/{id}
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

    // GET: api/suppliers/city/{cityId}
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

    // POST: api/suppliers
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

    // DELETE: api/suppliers/{id}
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
  }
}
