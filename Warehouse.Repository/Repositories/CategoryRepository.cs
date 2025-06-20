using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.Data.Repositories; // Assicurati che questo namespace sia corretto per GenericRepository
using Warehouse.Interfaces.IRepositories;
using System.Collections.Generic; // Necessario per IEnumerable
using System.Threading.Tasks;    // Necessario per Task

namespace Warehouse.Repository.Repositories
{
  public class CategoryRepository : GenericRepository<Categories>, ICategoryRepository
  {
    public CategoryRepository(WarehouseContext context) : base(context)
    {
    }

    public async Task<Categories?> GetCategoryByNameAsync(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        return null;

      // Usa _context.Set<Categories>() invece di _dbSet
      return await _context.Set<Categories>().FirstOrDefaultAsync(category => category.CategoryName == name);
    }

    public async Task<IEnumerable<Categories>> GetCategoriesWithProductsAsync()
    {
    
      return await _context.Categories
          .Include(c => c.Products)
          .ToListAsync();
    }

    public async Task<bool> CategoryNameExistsAsync(string name)
    {
      // Usa _context.Set<Categories>() invece di _dbSet
      return await _context.Set<Categories>().AnyAsync(category => category.CategoryName == name);
    }
  }
}
