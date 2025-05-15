using Warehouse.Data.Models;

namespace Warehouse.Interfaces.IRepositories
{

    public interface IOrderRepository : IGenericRepository<Orders>
    {
    // Mantieni solo i metodi specifici per gli ordini
    Task<IEnumerable<Orders>> GetAllAsync();
    Task<Orders> GetByIdAsync(int id);
    Task<Orders> GetOrderByIdAsync(int orderId);
    Task<System.Collections.Generic.IEnumerable<Orders>> GetOrdersByUserIdAsync(int userId);
    Task<System.Collections.Generic.IEnumerable<Orders>> GetOrdersBySupplierAsync(string supplierName);
    Task<System.Collections.Generic.IEnumerable<Orders>> GetOrdersByCategoryAsync(string categoryName);
    Task UpdateOrderAsync(Orders order);
    /*
        Task<Products> GetProductByIdAsync(int productId); // Per ottenere un prodotto di un ordine
        Task UpdateProductAsync(Products product); // Per aggiornare un prodotto in un ordine
    */
  }
}
