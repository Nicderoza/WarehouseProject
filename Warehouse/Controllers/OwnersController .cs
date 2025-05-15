using Microsoft.AspNetCore.Mvc;
using Warehouse.Common.DTOs;
using Warehouse.Interfaces.IServices;

namespace Warehouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerService _ownerService;

        public OwnersController(IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DTOOwner>> GetOwnerById(int id)
        {
            var owner = await _ownerService.GetOwnerByIdAsync(id);
            if (owner == null)
            {
                return NotFound();
            }
            return Ok(owner);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOOwner>>> GetAllOwners()
        {
            var owners = await _ownerService.GetAllOwnersAsync();
            return Ok(owners); 
        }


        [HttpPost]
        public async Task<ActionResult> AddOwner(DTOOwner ownerDTO)
        {
            await _ownerService.AddOwnerAsync(ownerDTO);
            return CreatedAtAction(nameof(GetOwnerById), new { id = ownerDTO.OwnerID }, ownerDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOwner(int id, DTOOwner ownerDTO)
        {
            if (id != ownerDTO.OwnerID)
            {
                return BadRequest();
            }
            await _ownerService.UpdateOwnerAsync(ownerDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOwner(int id)
        {
            await _ownerService.DeleteOwnerAsync(id);
            return NoContent();
        }
    }
}
