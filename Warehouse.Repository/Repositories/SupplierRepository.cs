using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.Data.Repositories;
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

    public async Task<Suppliers?> GetByNameAsync(string name) 
    {
      return await _context.Suppliers
          .FirstOrDefaultAsync(s => s.CompanyName == name);
    }

    public async Task<Suppliers?> GetSupplierByUserIdAsync(int userId)
    {
      return await _context.UsersSuppliers
                           .Where(us => us.UserID == userId)
                           .Select(us => us.Supplier)
                           .FirstOrDefaultAsync();
    }

    public async Task<int?> GetSupplierIdByUserIdAsync(int userId)
    {
      return await _context.UsersSuppliers
                           .Where(us => us.UserID == userId)
                           .Select(us => (int?)us.SupplierID)
                           .FirstOrDefaultAsync();
    }


    public async Task<List<Products>> GetProductsBySupplierIdAsync(int supplierId)
    {
      return await _context.Products
          .Where(p => p.SupplierID == supplierId)
          .ToListAsync();
    }
  }


}
