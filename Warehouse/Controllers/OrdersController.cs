using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Common.DTOs;
using Warehouse.Interfaces.IServices;

namespace Warehouse.WEB.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class OrdersController : ControllerBase
  {
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
      _orderService = orderService;
      _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
      var response = await _orderService.GetAllAsync();
      return response.Success ? Ok(response.Data) : BadRequest(response.Message);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
      var response = await _orderService.GetByIdAsync(id);
      if (response.Success) return Ok(response.Data);
      return response.NotFound ? NotFound(response.Message) : BadRequest(response.Message);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetOrdersByUserId(int userId)
    {
      var response = await _orderService.GetOrdersByUserIdAsync(userId);
      if (response.Success) return Ok(response.Data);
      return response.NotFound ? NotFound(response.Message) : BadRequest(response.Message);
    }

    [HttpGet("supplier/{supplierId}")]

    public async Task<IActionResult> GetOrdersBySupplier(int supplierID)

    {

      var response = await _orderService.GetOrdersBySupplierAsync(supplierID);

      if (response.Success) return Ok(response.Data);

      return response.NotFound ? NotFound(response.Message) : BadRequest(response.Message);

    }

    [HttpGet("category/{categoryName}")]
    public async Task<IActionResult> GetOrdersByCategory(string categoryName)
    {
      var response = await _orderService.GetOrdersByCategoryAsync(categoryName);
      if (response.Success) return Ok(response.Data);
      return response.NotFound ? NotFound(response.Message) : BadRequest(response.Message);
    }

    [HttpPut("{orderId}/status/{newStatus}")]
    public async Task<IActionResult> ChangeOrderStatus(int orderId, string newStatus)
    {
      var response = await _orderService.ChangeOrderStatusAsync(orderId, newStatus);
      if (response.Success) return Ok(response.Data);
      return response.NotFound ? NotFound(response.Message) : BadRequest(response.Message);
    }

    [HttpGet("{orderId}/details")]
    public async Task<IActionResult> GetOrderDetails(int orderId)
    {
      var response = await _orderService.GetOrderDetailsAsync(orderId);
      if (response.Success) return Ok(response.Data);
      return response.NotFound ? NotFound(response.Message) : BadRequest(response.Message);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] DTOOrderCreateRequest request)
    {
      if (request == null)
      {
        _logger.LogWarning("CreateOrder: richiesta nulla.");
        return BadRequest("Il corpo della richiesta è nullo.");
      }

      _logger.LogInformation("Richiesta ordine ricevuta {@Request}", request);

      if (!ModelState.IsValid)
      {
        _logger.LogWarning("CreateOrder: model non valido. Errori: {Errors}",
          string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
        return BadRequest(ModelState);
      }

      try
      {
        var result = await _orderService.CreateOrderAsync(request);
        if (!result.Success)
        {
          _logger.LogWarning("Errore nella creazione ordine: {Message}", result.Message);
          return BadRequest(result.Message);
        }

        return CreatedAtAction(nameof(GetOrderById), new { id = result.Data.OrderID }, result.Data);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Errore interno durante la creazione dell'ordine.");
        return StatusCode(500, "Errore interno del server");
      }
    }

    [Authorize]
    [HttpPost("checkout")]
    public async Task<IActionResult> CheckoutFromCart()
    {
      var userId = GetUserIdFromToken();
      if (userId == 0)
      {
        _logger.LogWarning("CheckoutFromCart: utente non autenticato.");
        return Unauthorized("Utente non autenticato.");
      }

      var result = await _orderService.CheckoutFromCartAsync(userId);
      if (result.Success) return Ok(result.Data);
      return result.NotFound ? NotFound(result.Message) : BadRequest(result.Message);
    }

    private int GetUserIdFromToken()
    {
      var userIdClaim = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier);
      return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
    }
  }
}
