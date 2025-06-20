using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.Data.Repositories;
using Warehouse.Interfaces.IRepositories;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Repository.Repositories
{
  public class OrderRepository : GenericRepository<Orders>, IOrderRepository
  {
    private IDbContextTransaction? _currentTransaction;

    public OrderRepository(WarehouseContext context) : base(context) { }

    public override async Task<IEnumerable<Orders>> GetAllAsync()
    {
      return await _context.Orders
                           .Include(o => o.OrderItems)
                               .ThenInclude(oi => oi.Product)
                           .ToListAsync();
    }

    public override async Task<Orders?> GetByIdAsync(int id)
    {
      return await _context.Orders
                           .Include(o => o.OrderItems)
                               .ThenInclude(oi => oi.Product)
                           .FirstOrDefaultAsync(o => o.OrderID == id);
    }

    public async Task<Orders?> GetOrderByIdAsync(int orderId)
    {
      return await _context.Orders
                           .Include(o => o.OrderStatus)
                           .Include(o => o.OrderItems)
                               .ThenInclude(oi => oi.Product)
                                   .ThenInclude(p => p.Supplier)
                           .Include(o => o.OrderItems)
                               .ThenInclude(oi => oi.Product)
                                   .ThenInclude(p => p.Category)
                           .FirstOrDefaultAsync(o => o.OrderID == orderId);
    }

    public async Task<IEnumerable<Orders>> GetOrdersByUserIdAsync(int userId)
    {
      return await _context.Orders
                           .Where(o => o.UserID == userId)
                     .Include(o => o.OrderStatus) 
                     .Include(o => o.OrderItems)
                         .ThenInclude(oi => oi.Product)
                             .ThenInclude(p => p.Category)
                     .Include(o => o.OrderItems)
                         .ThenInclude(oi => oi.Product)
                             .ThenInclude(p => p.Supplier)
                     .ToListAsync();
    }


    public async Task<IEnumerable<Orders>> GetOrdersBySupplierAsync(int  supplierId)
    {
      return await _context.Orders
                           .Where(o => o.OrderItems.Any(oi => oi.Product != null &&
                                                              oi.Product.Supplier != null &&
                                                              oi.Product.Supplier.SupplierID == supplierId))
                           .Include(o => o.OrderItems)
                               .ThenInclude(oi => oi.Product)
                           .ToListAsync();
    }

    public async Task<IEnumerable<Orders>> GetOrdersByCategoryAsync(string categoryName)
    {
      return await _context.Orders
                           .Where(o => o.OrderItems.Any(oi => oi.Product != null &&
                                                              oi.Product.Category != null &&
                                                              oi.Product.Category.CategoryName == categoryName))
                           .Include(o => o.OrderItems)
                               .ThenInclude(oi => oi.Product)
                           .ToListAsync();
    }

    public async Task<BaseResponse<DTOOrder>> CheckoutFromCartAsync(int userId)
    {
      // Stub: Implement business logic for converting cart to order.
      // Example response:
      return new BaseResponse<DTOOrder>
      {
        Success = false,
        Message = "Checkout functionality not implemented yet.",
        Data = null
      };
    }

    public async Task UpdateOrderAsync(Orders order)
    {
      _context.Orders.Update(order);
      await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
      if (_currentTransaction != null)
        return;

      _currentTransaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
      if (_currentTransaction != null)
      {
        await _currentTransaction.CommitAsync();
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
      }
    }

    public async Task RollbackTransactionAsync()
    {
      if (_currentTransaction != null)
      {
        await _currentTransaction.RollbackAsync();
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
      }
    }
    public async Task<List<OrderItems>> GetOrderItemsWithDetailsAsync(int orderId)
    {
      return await _context.OrderItems
          .Include(oi => oi.Product)
              .ThenInclude(p => p.Category)
          .Include(oi => oi.Product)
              .ThenInclude(p => p.Supplier)
          .Where(oi => oi.OrderID == orderId)
          .ToListAsync();
    }

  }
}
