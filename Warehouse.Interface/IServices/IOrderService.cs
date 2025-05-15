using Warehouse.Common.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Common.DTOs;
using Warehouse.Common.DTOs;
using Warehouse.Data.Models;
namespace Warehouse.Interfaces.IServices
{
  public interface IOrderService : IGenericService<DTOOrder>
  {
    Task<BaseResponse<IEnumerable<DTOOrder>>> GetOrdersByUserIdAsync(int userId);
    Task<BaseResponse<IEnumerable<DTOOrder>>> GetOrdersBySupplierAsync(string supplierName);
    Task<BaseResponse<IEnumerable<DTOOrder>>> GetOrdersByCategoryAsync(string categoryName);
    Task<BaseResponse<DTOOrder>> ChangeOrderStatusAsync(int orderId, string newStatus);
    Task<BaseResponse<DTOOrder>> GetOrderDetailsAsync(int orderId);
    Task<BaseResponse<IEnumerable<DTOOrder>>> GetAllAsync();
    Task<BaseResponse<DTOOrder>> GetByIdAsync(int id);
  }
}
