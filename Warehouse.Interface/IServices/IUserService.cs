using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;

namespace Warehouse.Interfaces.IServices
{
  public interface IUserService : IGenericService<DTOUser>
  {
    Task<DTOUser> GetByEmailAsync(string email);
    Task<BaseResponse<DTOUser>> CreateAsync(DTOCreateUser createUserDto); // Aggiorna la firma del metodo
  }
}
