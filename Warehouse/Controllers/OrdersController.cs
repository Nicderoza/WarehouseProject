using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Common.DTOs;
using Warehouse.Interfaces.IServices;

namespace Warehouse.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class OrdersController : ControllerBase
  {
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
      _orderService = orderService;
    }

    // GET: api/orders
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
      var response = await _orderService.GetAllAsync();
      if (response.Success)
      {
        return Ok(response.Data);
      }
      else
      {
        return BadRequest(response.Message);
      }
    }

    // GET: api/orders/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
      var response = await _orderService.GetByIdAsync(id);
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

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetOrdersByUserId(int userId)
    {
      var response = await _orderService.GetOrdersByUserIdAsync(userId);
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

    [HttpGet("supplier/{supplierName}")]
    public async Task<IActionResult> GetOrdersBySupplier(string supplierName)
    {
      var response = await _orderService.GetOrdersBySupplierAsync(supplierName);
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

    [HttpGet("category/{categoryName}")]
    public async Task<IActionResult> GetOrdersByCategory(string categoryName)
    {
      var response = await _orderService.GetOrdersByCategoryAsync(categoryName);
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

    [HttpPut("{orderId}/status/{newStatus}")]
    public async Task<IActionResult> ChangeOrderStatus(int orderId, string newStatus)
    {
      var response = await _orderService.ChangeOrderStatusAsync(orderId, newStatus);
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

    [HttpGet("{orderId}/details")] // Modifica l'attributo [HttpGet]
    public async Task<IActionResult> GetOrderDetails(int orderId)
    {
      var response = await _orderService.GetOrderDetailsAsync(orderId);
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
  }
}
