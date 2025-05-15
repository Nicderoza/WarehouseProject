using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using System.Threading.Tasks;

namespace Warehouse.Repository.Repositories
{
  public class CityRepository : GenericRepository<Cities>, ICityRepository
  {
    public CityRepository(WarehouseContext context) : base(context)
    {
    }

    public async Task<Cities?> GetCityByNameAsync(string name)
    {
      return await _dbSet.FirstOrDefaultAsync(city => city.Name == name);
    }

    public async Task<bool> CityNameExistsAsync(string name)
    {
      return await _dbSet.AnyAsync(city => city.Name == name);
    }
  }
}
