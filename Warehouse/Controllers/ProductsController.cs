using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Interfaces.IServices;

namespace Warehouse.WEB.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase
  {
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductsController(IProductService productService, IMapper mapper)
    {
      _productService = productService;
      _mapper = mapper;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DTOProduct>>> GetAllProducts()
    {
      var products = await _productService.GetAllAsync();
      return Ok(products);
    }

    // GET: api/products/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DTOProduct>> GetProductById(int id)
    {
      var product = await _productService.GetByIdAsync(id);
      return product != null ? Ok(product) : NotFound();
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<BaseResponse<DTOProduct>>> CreateProduct([FromBody] DTOProduct productDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var response = await _productService.AddAsync(productDto);
      return CreatedAtAction(nameof(GetProductById), new { id = response.Data.ProductID }, response);
    }

    // PUT: api/products/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] DTOProduct productDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      if (id != productDto.ProductID)
        return BadRequest("Product ID mismatch");

      var response = await _productService.UpdateAsync(id, productDto);
      return response.Success ? NoContent() : NotFound(response);
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
      var response = await _productService.DeleteAsync(id);
      return response.Success ? NoContent() : NotFound(response);
    }

    // GET: api/products/category/{categoryId}
    [HttpGet("category/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<DTOProduct>>> GetProductsByCategory(int categoryId)
    {
      var products = await _productService.GetProductsByCategoryAsync(categoryId);
      return Ok(products);
    }

    // GET: api/products/details/{id}
    [HttpGet("details/{id:int}")]
    public async Task<ActionResult<DTOProduct>> GetProductDetails(int id)
    {
      var productDetails = await _productService.GetProductDetailsAsync(id);
      return productDetails != null ? Ok(productDetails) : NotFound();
    }
  }
}
