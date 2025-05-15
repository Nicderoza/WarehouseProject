using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;

namespace Warehouse.Repository.Repositories
{
  public class CategoryRepository : GenericRepository<Categories>, ICategoryRepository
  {
    public CategoryRepository(WarehouseContext context) : base(context) { }

    public async Task<Categories> GetCategoryByNameAsync(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        return null;

      return await _dbSet.FirstOrDefaultAsync(category => category.CategoryName == name);
    }

    public async Task<IEnumerable<Categories>> GetCategoriesWithProductsAsync()
    {
      return await _context.Categories
          .Include(c => c.Products)
          .ToListAsync();
    }

    public async Task<bool> CategoryNameExistsAsync(string name)
    {
      return await _dbSet.AnyAsync(category => category.CategoryName == name);
    }
  }
}
