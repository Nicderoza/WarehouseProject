namespace Warehouse.Interfaces.IRepositories
{ 
  public interface IOrderStatusRepository
  {
    Task<OrderStatus?> GetByNameAsync(string statusName);
    Task<IEnumerable<OrderStatus>> GetAllAsync();
    Task<OrderStatus> GetByIdAsync(int id);

  }
}
