// Warehouse.Interfaces.IServices/IOrderService.cs
using Warehouse.Common.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Common.DTOs;
using Warehouse.Data.Models;

namespace Warehouse.Interfaces.IServices
{
  public interface IOrderService : IGenericService<DTOOrder>
  {
    Task<BaseResponse<DTOOrder>> CreateOrderAsync(DTOOrderCreateRequest createOrderDto);
    Task<BaseResponse<IEnumerable<DTOOrder>>> GetOrdersByUserIdAsync(int userId);
    Task<BaseResponse<IEnumerable<DTOOrder>>> GetOrdersBySupplierAsync(int supplierId);
    Task<BaseResponse<IEnumerable<DTOOrder>>> GetOrdersByCategoryAsync(string categoryName);
    Task<BaseResponse<DTOOrder>> ChangeOrderStatusAsync(int orderId, string newStatusName);
    Task<BaseResponse<DTOOrder>> GetOrderDetailsAsync(int orderId);
    Task<BaseResponse<DTOOrder>> CheckoutFromCartAsync(int userId);
  }
}
