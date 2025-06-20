using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Interfaces;
using Warehouse.Data.Models;

namespace Warehouse.Data.Repositories
{
  public class UsersSuppliersRepository : IUsersSuppliersRepository
  {
    private readonly WarehouseContext _context;

    public UsersSuppliersRepository(WarehouseContext context)
    {
      _context = context;
    }
    public async Task AddUsersSuppliersAsync(UsersSuppliers usersSuppliers)
    {
      await _context.UsersSuppliers.AddAsync(usersSuppliers);
      await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UsersSuppliers>> GetAllUsersSuppliersAsync()
    {
      return await _context.UsersSuppliers
                           .Include(us => us.User)
                           .Include(us => us.Supplier)
                           .ToListAsync();
    }

    public async Task<UsersSuppliers> GetUsersSuppliersByIdAsync(int userId, int supplierId)
    {
      return await _context.UsersSuppliers
                           .Include(us => us.User)
                           .Include(us => us.Supplier)
                           .FirstOrDefaultAsync(us => us.UserID == userId && us.SupplierID == supplierId);
    }

    public async Task UpdateUsersSuppliersAsync(UsersSuppliers usersSuppliers)
    {
      // Attach l'entità al contesto e imposta lo stato su Modified
      _context.Entry(usersSuppliers).State = EntityState.Modified;
      await _context.SaveChangesAsync(); // Salva subito le modifiche
    }

    public async Task DeleteUsersSuppliersAsync(int userId, int supplierId)
    {
      var usersSuppliers = await _context.UsersSuppliers
                                       .FirstOrDefaultAsync(us => us.UserID == userId && us.SupplierID == supplierId);
      if (usersSuppliers != null)
      {
        _context.UsersSuppliers.Remove(usersSuppliers);
        await _context.SaveChangesAsync();
      }
    }
    public async Task<bool> SaveChangesAsync()
    {
      return await _context.SaveChangesAsync() > 0;
    }
    public async Task<UsersSuppliers?> GetUsersSuppliersByUserIdAsync(int userId)
    {
      return await _context.UsersSuppliers
          .FirstOrDefaultAsync(us => us.UserID == userId);
    }

  }
}
