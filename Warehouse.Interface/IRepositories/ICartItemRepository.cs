
  using System.Collections.Generic; // Aggiunto per Task<List<CartItem>>
  // Warehouse.Interfaces.IRepositories/ICartItemRepository.cs
  using System.Threading.Tasks;
  using global::Warehouse.Data.Models;

  namespace Warehouse.Interfaces.IRepositories
  {
    // Eredita dall'interfaccia generica per le operazioni CRUD comuni
    // Assumo che IGenericRepository<T> sia già definita nel tuo progetto Interfaces.IRepositories
    public interface ICartItemRepository : IGenericRepository<CartItems>
    {
      // Metodo specifico per ottenere un articolo del carrello tramite ID del carrello e ID del prodotto
      Task<CartItems> GetCartItemByCartIdAndProductIdAsync(int cartId, int productId);

      // Metodo per ottenere tutti gli articoli di un carrello specifico
      Task<List<CartItems>> GetCartItemsByCartIdAsync(int cartId);
    }
  }
