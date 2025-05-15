using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Warehouse.Common.DTOs;
using Warehouse.Interfaces.IServices;
using System.Threading.Tasks;

namespace Warehouse.WEB.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class CitiesController : ControllerBase
  {
    private readonly ICityService _cityService;
    private readonly ILogger<CitiesController> _logger;

    public CitiesController(ICityService cityService, ILogger<CitiesController> logger)
    {
      _cityService = cityService;
      _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCities()
    {
      var response = await _cityService.GetAllAsync();

      if (response == null)
      {
        _logger.LogError("CityService.GetAllAsync() returned null.");
        return StatusCode(500, "Errore interno nel recuperare le città.");
      }

      if (response.Success)
      {
        return Ok(response.Data);
      }

      return BadRequest(response.Message);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCityById(int id)
    {
      var response = await _cityService.GetByIdAsync(id);
      if (response.Success)
      {
        return Ok(response.Data);
      }
      return response.NotFound ? NotFound(response.Message) : BadRequest(response.Message);
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetCityByName(string name)
    {
      var response = await _cityService.GetCityByNameAsync(name);
      if (response.Success)
      {
        return Ok(response.Data);
      }
      return response.NotFound ? NotFound(response.Message) : BadRequest(response.Message);
    }
  }
}
