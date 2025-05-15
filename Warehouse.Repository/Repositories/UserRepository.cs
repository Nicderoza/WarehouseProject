using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using System.Threading.Tasks; // Make sure you have this!
using System.Linq; // We need this to use 'FirstOrDefaultAsync'

namespace Warehouse.Repository.Repositories
{
  public class UserRepository : GenericRepository<Users>, IUserRepository
  {
    private readonly WarehouseContext _context; // We need to be able to talk to the database

    public UserRepository(WarehouseContext context) : base(context)
    {
      _context = context;
    }

    public async Task<Users> GetByEmailAsync(string email)
    {
      return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

  }
}
