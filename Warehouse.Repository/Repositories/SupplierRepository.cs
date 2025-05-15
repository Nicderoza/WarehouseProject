using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;

namespace Warehouse.Repository.Repositories
{
  public class SupplierRepository : GenericRepository<Suppliers>, ISupplierRepository
  {
    private readonly WarehouseContext _context;

    public SupplierRepository(WarehouseContext context) : base(context)
    {
      _context = context;
    }
    public async Task<IEnumerable<Suppliers>> GetSuppliersByCityAsync(int cityId)
    {
      return await _context.Suppliers
          .Where(s => s.CityID == cityId)
          .ToListAsync();
    }
  }
}
