using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data.Models;

namespace Warehouse.Interfaces.IRepositories
{
  public interface IOrderRepository : IGenericRepository<Orders>
  {
    Task<IEnumerable<Orders>> GetAllAsync();
    Task<Orders?> GetByIdAsync(int id);
    Task<Orders?> GetOrderByIdAsync(int orderId);
    Task<IEnumerable<Orders>> GetOrdersByUserIdAsync(int userId);
    Task<IEnumerable<Orders>> GetOrdersBySupplierAsync(int supplierId);
    Task<IEnumerable<Orders>> GetOrdersByCategoryAsync(string categoryName);
    Task<BaseResponse<DTOOrder>> CheckoutFromCartAsync(int userId);
    Task UpdateOrderAsync(Orders order);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<List<OrderItems>> GetOrderItemsWithDetailsAsync(int orderId);
    Task<Orders> CreateOrderAsync(Orders order);
  }
}
