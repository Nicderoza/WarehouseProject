using Warehouse.Data.Models;
using System.Threading.Tasks; // Make sure you have this!

namespace Warehouse.Interfaces.IRepositories
{
  public interface IUserRepository : IGenericRepository<Users>
  {
    Task<Users> GetByEmailAsync(string email); // We expect to get a User back!
  }
}
