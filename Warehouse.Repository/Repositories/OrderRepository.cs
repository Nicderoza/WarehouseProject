using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;

namespace Warehouse.Repository.Repositories
{
  public class OrderRepository : GenericRepository<Orders>, IOrderRepository
  {
    private readonly WarehouseContext _context;

    public OrderRepository(WarehouseContext context) : base(context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Orders>> GetAllAsync()
    {
      return await _context.Orders.ToListAsync();
    }

    public async Task<Orders> GetByIdAsync(int id)
    {
      return await _context.Orders.FindAsync(id);
    }
    public async Task<IEnumerable<Orders>> GetOrdersByUserIdAsync(int userId)
    {
      return await _context.Orders
          .Where(o => o.UserID == userId)
          .ToListAsync();
    }

    public async Task<IEnumerable<Orders>> GetOrdersBySupplierAsync(string supplierName)
    {
      return await _context.Orders
          .Where(o => o.OrderItems.Any(oi => oi.Product.Supplier.Name == supplierName))
          .ToListAsync();
    }
    public async Task<IEnumerable<Orders>> GetOrdersByCategoryAsync(string categoryName)
    {
      return await _context.Orders
          .Where(o => o.OrderItems.Any(oi => oi.Product.Category.CategoryName == categoryName))
          .ToListAsync();
    }
    public async Task<Products> GetProductByIdAsync(int productId)
    {
      return await _context.Products.FindAsync(productId);
    }

    public async Task UpdateProductAsync(Products product)
    {
      _context.Products.Update(product);
      await _context.SaveChangesAsync();
    }

    public async Task<Orders> GetOrderByIdAsync(int orderId)
    {
      return await _context.Orders.FindAsync(orderId);
    }

    public async Task UpdateOrderAsync(Orders order)
    {
      _context.Orders.Update(order);
      await _context.SaveChangesAsync();
    }
  }
}
