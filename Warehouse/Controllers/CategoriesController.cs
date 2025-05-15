using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Interfaces.IServices;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
  private readonly ICategoryService _categoryService;

  public CategoriesController(ICategoryService categoryService)
  {
    _categoryService = categoryService;
  }

  [HttpGet]
  public async Task<IActionResult> GetAllCategoriesWithProducts()
  {
    BaseResponse<IEnumerable<DTOCategory>> response = await _categoryService.GetCategoriesWithProductsAsync();
    if (response.Success)
    {
      return Ok(response.Data);
    }
    return NotFound(response.Message);
  }

  [HttpGet("name/{name}")]
  public async Task<IActionResult> GetCategoryByName(string name)
  {
    BaseResponse<DTOCategory> response = await _categoryService.GetCategoryByNameAsync(name);
    if (response.Success)
    {
      return Ok(response.Data);
    }
    return NotFound(response.Message);
  }

  [HttpGet("all")]
  [Route("api/Categories/all")] // Aggiungi questa riga
  public async Task<IActionResult> GetAllCategories()
  {
    var response = await _categoryService.GetAllAsync();
    if (response.Success)
    {
      return Ok(response.Data);
    }
    return BadRequest(response.Message); // Modifica qui
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetCategoryById(int id)
  {
    var response = await _categoryService.GetByIdAsync(id);
    if (response.Success)
    {
      return Ok(response.Data);
    }
    return response.NotFound ? NotFound(response.Message) : BadRequest(response.Message);
  }
}
