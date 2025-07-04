using Warehouse.Data.Models;
using System.Threading.Tasks;  

namespace Warehouse.Interfaces.IRepositories
{
  public interface IUserRepository : IGenericRepository<Users>
  {
    Task<Users> GetByEmailAsync(string email);
    Task<Users?> GetByIdAsync(int userId);
    Task<Users> ToggleStatusAsync(int userId);
    Task AssociateSupplierAsync(int userId, int supplierId);
    Task DissociateSupplierAsync(int userId, int supplierId);
    Task ChangeRoleAsync(int userId, int newRoleId);
    Task<List<Users>> GetUsersWithoutSupplierAsync();
    Task<List<Users>> GetUsersWithSuppliersAsync();
    Task<List<Users>> GetUsersBySupplierAsync(int supplierId);


  }
}
