using global::Warehouse.Data.Models;

  namespace Warehouse.Interfaces.IRepositories
  {
    // Eredita dall'interfaccia generica per i metodi CRUD base
    public interface ICartRepository : IGenericRepository<Cart>
    {
      // Metodi specifici per il carrello che richiedono logica personalizzata di query
      Task<Cart> GetActiveCartByUserIdAsync(int userId);
      Task<CartItems> GetCartItemByCartIdAndProductIdAsync(int cartId, int productId);
    }
  }
