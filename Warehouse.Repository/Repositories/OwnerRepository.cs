using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;

namespace Warehouse.Repository.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly WarehouseContext _context;

        public OwnerRepository(WarehouseContext context)
        {
            _context = context;
        }

        public async Task<Owners> GetOwnerByIdAsync(int id)
        {
            return await _context.Owners.FindAsync(id);
        }

        public async Task<IEnumerable<Owners>> GetAllOwnersAsync()
        {
            return await _context.Owners.ToListAsync();
        }

        public async Task AddOwnerAsync(Owners owner)
        {
            await _context.Owners.AddAsync(owner);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOwnerAsync(Owners owner)
        {
            _context.Owners.Update(owner);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOwnerAsync(int id)
        {
            var owner = await GetOwnerByIdAsync(id);
            if (owner != null)
            {
                _context.Owners.Remove(owner);
                await _context.SaveChangesAsync();
            }
        }
    }
}
