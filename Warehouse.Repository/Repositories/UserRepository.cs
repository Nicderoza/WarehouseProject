using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.Data.Repositories;
using Warehouse.Interfaces.IRepositories;

namespace Warehouse.Repository.Repositories
{
  public class UserRepository : GenericRepository<Users>, IUserRepository
  {
    private readonly WarehouseContext _context; 

    public UserRepository(WarehouseContext context) : base(context)
    {
      _context = context;
    }

    public async Task<Users> GetByEmailAsync(string email)
    {
      return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Users> ToggleStatusAsync(int userId)
    {
      var user = await _context.Users.FindAsync(userId)
                 ?? throw new KeyNotFoundException("Utente non trovato");

      user.IsActive = !user.IsActive;
      await _context.SaveChangesAsync();
      return user;
    }

    public async Task AssociateSupplierAsync(int userId, int supplierId)
    {
      var exists = await _context.UsersSuppliers
        .AnyAsync(us => us.UserID == userId && us.SupplierID == supplierId);

      if (!exists)
      {
        await _context.UsersSuppliers.AddAsync(new UsersSuppliers
        {
          UserID = userId,
          SupplierID = supplierId
        });
        await _context.SaveChangesAsync();
      }
    }

    public async Task DissociateSupplierAsync(int userId, int supplierId)
    {
      var link = await _context.UsersSuppliers
          .FirstOrDefaultAsync(us => us.UserID == userId && us.SupplierID == supplierId);

      if (link != null)
      {
        _context.UsersSuppliers.Remove(link);
        await _context.SaveChangesAsync();
      }
    }


    public async Task ChangeRoleAsync(int userId, int newRoleId)
    {
      var user = await _context.Users
          .Include(u => u.Role)
          .FirstOrDefaultAsync(u => u.UserID == userId)
          ?? throw new KeyNotFoundException("Utente non trovato");

      var role = await _context.Roles
          .FirstOrDefaultAsync(r => r.RoleID == newRoleId)
          ?? throw new KeyNotFoundException("Ruolo non trovato");

      user.Role = role;
      user.RoleID = role.RoleID;

      await _context.SaveChangesAsync();
    }


    public async Task<List<Users>> GetUsersWithSuppliersAsync()
    {
      return await _context.Users
          .Where(u => _context.UsersSuppliers.Any(us => us.UserID == u.UserID)
                      && u.RoleID == 2)
          .ToListAsync();
    }

    public async Task<List<Users>> GetUsersWithoutSupplierAsync()
    {
      return await _context.Users
          .Where(u => !_context.UsersSuppliers.Any(us => us.UserID == u.UserID)
                      && u.RoleID == 3) // Solo clienti
          .ToListAsync();
    }

    public async Task<List<Users>> GetUsersBySupplierAsync(int supplierId)
    {
      return await _context.Users
     .Where(u => u.RoleID == 2 && _context.UsersSuppliers
         .Any(us => us.UserID == u.UserID && us.SupplierID == supplierId))
     .ToListAsync();

    }

  }
}
