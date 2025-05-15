using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using System.Threading.Tasks;

namespace Warehouse.Repository.Repositories
{
  public class ProductRepository : GenericRepository<Products>, IProductRepository
  {
    private readonly WarehouseContext _context;

    public ProductRepository(WarehouseContext context) : base(context)
    {
      _context = context;
    }

    public async Task<Products> GetProductDetailsAsync(int productId)
    {
      return await _context.Products.FindAsync(productId);
    }

    public async Task<IEnumerable<Products>> GetProductsByCategoryAsync(int categoryId)
    {
      return await _context.Products
          .Where(p => p.CategoryID == categoryId)
          .ToListAsync();
    }

    public async Task<IEnumerable<Products>> GetProductsByOrderIdAsync(int orderId)
    {
      return await _context.OrderItems
          .Where(oi => oi.OrderID == orderId)
          .Select(oi => oi.Product)
          .ToListAsync();
    }

    public async Task<Products> GetProductByIdAsync(int id)
    {
      return await _context.Products.FindAsync(id);
    }

    public async Task UpdateProductAsync(Products product)
    {
      _context.Entry(product).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }
  }
}
