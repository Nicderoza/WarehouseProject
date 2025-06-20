using Warehouse.Common.DTOs;
using Warehouse.Data.Models;

namespace Warehouse.Interfaces.IRepositories
{
  public interface IProductRepository : IGenericRepository<Products>
  {
    Task<Products> GetProductDetailsAsync(int productId);
    Task<IEnumerable<Products>> GetProductsByCategoryAsync(int categoryId);

    Task<IEnumerable<Products>> GetProductsByOrderIdAsync(int orderId);
    Task UpdateProductAsync(Products product);
    Task<IEnumerable<Products>> GetByIdsAsync(IEnumerable<int> productIds);
    Task<IEnumerable<Products>> GetProductsByUserSupplierAsync(int userId);



  }

}
