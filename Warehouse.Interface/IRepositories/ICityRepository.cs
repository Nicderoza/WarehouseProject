using Warehouse.Data.Models;
using System.Threading.Tasks;

namespace Warehouse.Interfaces.IRepositories
{
    public interface ICityRepository : IGenericRepository<Cities>
    {
        Task<Cities?> GetCityByNameAsync(string name);
        Task<bool> CityNameExistsAsync(string name);
  }
}
