using Warehouse.Data.Models;

namespace Warehouse.Interfaces.IRepositories
{
    public interface ISupplierRepository : IGenericRepository<Suppliers>
    {

        Task<IEnumerable<Suppliers>> GetSuppliersByCityAsync(int cityId);

    }
}
