// File: Warehouse.Repository/Repositories/CityRepository.cs

using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.Data.Repositories;
using Warehouse.Interfaces.IRepositories;

namespace Warehouse.Repository.Repositories
{
  public class CityRepository : GenericRepository<Cities>, ICityRepository
  {
    public CityRepository(WarehouseContext context) : base(context)
    {
    }

    public async Task<Cities?> GetCityByNameAsync(string name)
    {
      return await _context.Set<Cities>()
                           .FirstOrDefaultAsync(city => city.Name == name);
    }

    public async Task<bool> CityNameExistsAsync(string name)
    {
      return await _context.Set<Cities>()
                           .AnyAsync(city => city.Name == name);
    }
  }
}
