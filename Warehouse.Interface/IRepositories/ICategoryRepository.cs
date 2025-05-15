using Warehouse.Data.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Warehouse.Interfaces.IRepositories
{
  public interface ICategoryRepository : IGenericRepository<Categories>
  {
    Task<Categories> GetCategoryByNameAsync(string name);
    Task<IEnumerable<Categories>> GetCategoriesWithProductsAsync();
    Task<bool> CategoryNameExistsAsync(string name); // Aggiungi questo metodo
  }
}
