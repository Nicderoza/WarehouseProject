using Warehouse.Common.DTOs;

using System.Collections.Generic;
using System.Threading.Tasks;


namespace Warehouse.Interfaces.IServices
{
    public interface IProductService : IGenericService<DTOProduct>
    {
    Task<IEnumerable<DTOProduct>> GetProductsByCategoryAsync(int categoryId);
    Task<DTOProduct?> GetProductDetailsAsync(int id);
    Task<IEnumerable<DTOProduct>> GetProductsByOrderIdAsync(int orderId);
  }

}
