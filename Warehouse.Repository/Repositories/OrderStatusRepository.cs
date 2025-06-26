using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Interfaces.IRepositories;

namespace Warehouse.Repository.Repositories
{
  public class OrderStatusRepository : IOrderStatusRepository
  {
    private readonly WarehouseContext _context;

    public OrderStatusRepository(WarehouseContext context)
    {
      _context = context;
    }

    public async Task<OrderStatus?> GetByNameAsync(string statusName)
    {
      return await _context.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == statusName);
    }

    public async Task<IEnumerable<OrderStatus>> GetAllAsync()
    {
      return await _context.OrderStatuses.ToListAsync();
    }
    public async Task<OrderStatus> GetByIdAsync(int id)
    {
      return await _context.OrderStatuses.FindAsync(id);
    }

  }

}
