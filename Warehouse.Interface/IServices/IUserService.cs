using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data.Models;


namespace Warehouse.Interfaces.IServices
{
  public interface IUserService : IGenericService<DTOUser> 
  {
    Task<DTOUser> GetByEmailAsync(string email);
    Task<BaseResponse<DTOUser>> CreateAsync(DTOCreateUser createUserDto);

    Task<BaseResponse<DTOUser>> GetUserByIdAsync(int id);

    Task<BaseResponse<DTOUser>> UpdateUserAsync(int id, DTOUser userDto); 

    Task<BaseResponse<string>> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    Task<DTOUser> ToggleStatusAsync(int userId);
    Task AssociateSupplierAsync(int userId, int supplierId);
    Task DissociateSupplierAsync(int userId, int supplierId);
    Task ChangeRoleAsync(int userId, string newRole);

    Task<List<DTOUser>> GetUsersWithoutSupplierAsync();
    Task<List<DTOUser>> GetUsersWithSuppliersAsync();
    Task<List<DTOUser>> GetUsersBySupplierAsync(int supplierId);



  }
}
