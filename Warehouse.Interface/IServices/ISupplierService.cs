using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;

namespace Warehouse.Interfaces.IServices
{
    public interface ISupplierService : IGenericService<DTOSupplier>
    {
        Task<BaseResponse<IEnumerable<DTOSupplier>>> GetSuppliersByCityAsync(int cityId);
        Task<BaseResponse<DTOSupplier>> CreateSupplierProfileAsync(int userId, string companyName, int cityId);
    //Task<TEntity> AddAsync(TEntity entity);
    Task<DTOSupplier?> GetByNameAsync(string companyName);
    Task<List<DTOProduct>> GetProductsByUserIdAsync(int userId);
    Task<DTOSupplier?> GetSupplierByUserIdAsync(int userId);


  }
}
