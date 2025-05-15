using Warehouse.Common.DTOs;

namespace Warehouse.Interfaces.IServices
{
    public interface IOwnerService
    {
        Task<IEnumerable<DTOOwner>> GetAllOwnersAsync();
        Task<DTOOwner> GetOwnerByIdAsync(int id);
        Task AddOwnerAsync(DTOOwner ownerDTO);
        Task UpdateOwnerAsync(DTOOwner ownerDTO);
        Task DeleteOwnerAsync(int id);
    }
}
