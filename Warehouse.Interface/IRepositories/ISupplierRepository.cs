using Warehouse.Data.Models;

namespace Warehouse.Interfaces.IRepositories
{
  public interface ISupplierRepository : IGenericRepository<Suppliers>
  {
    Task<IEnumerable<Suppliers>> GetSuppliersByCityAsync(int cityId);
    Task<Suppliers?> GetByNameAsync(string name);
    Task<Suppliers?> GetSupplierByUserIdAsync(int userId);
    Task<int?> GetSupplierIdByUserIdAsync(int userId);
    Task<List<Products>> GetProductsBySupplierIdAsync(int supplierId);
  }
}
