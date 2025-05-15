using Warehouse.Data.Models;

namespace Warehouse.Interfaces.IRepositories
{
    public interface IOwnerRepository
    {
        Task<Owners> GetOwnerByIdAsync(int id);
        Task<IEnumerable<Owners>> GetAllOwnersAsync();
        Task AddOwnerAsync(Owners owner);
        Task UpdateOwnerAsync(Owners owner);
        Task DeleteOwnerAsync(int id);
    }
}
