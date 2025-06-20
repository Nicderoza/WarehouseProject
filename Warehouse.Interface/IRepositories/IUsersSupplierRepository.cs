
using Warehouse.Data.Models;

namespace Warehouse.Data.Interfaces
{
  public interface IUsersSuppliersRepository
  {
    Task<IEnumerable<UsersSuppliers>> GetAllUsersSuppliersAsync();
    Task<UsersSuppliers> GetUsersSuppliersByIdAsync(int userId, int supplierId);
    Task AddUsersSuppliersAsync(UsersSuppliers usersSuppliers);
    Task UpdateUsersSuppliersAsync(UsersSuppliers usersSuppliers);
    Task<UsersSuppliers?> GetUsersSuppliersByUserIdAsync(int userId);

    Task DeleteUsersSuppliersAsync(int userId, int supplierId);
    Task<bool> SaveChangesAsync();
  }
}
