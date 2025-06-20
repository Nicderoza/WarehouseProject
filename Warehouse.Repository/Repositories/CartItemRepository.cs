// Warehouse.Data.Repositories/CartItemRepository.cs
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Models; // Include CartItem, Product, CartStatus
using Warehouse.Interfaces.IRepositories; // Assumi che questo sia il namespace per ICartItemRepository

namespace Warehouse.Data.Repositories
{
  public class CartItemRepository : GenericRepository<CartItems>, ICartItemRepository
  {
    private readonly WarehouseContext _context;

    public CartItemRepository(WarehouseContext context) : base(context)
    {
      _context = context;
    }

    public async Task<CartItems> GetCartItemByCartIdAndProductIdAsync(int cartId, int productId)
    {
      return await _context.CartItems
                           .Include(ci => ci.Product) // Includi il prodotto per i dettagli
                           .FirstOrDefaultAsync(ci => ci.CartID == cartId && ci.ProductID == productId);
    }

    public async Task<List<CartItems>> GetCartItemsByCartIdAsync(int cartId)
    {
      return await _context.CartItems
                           .Where(ci => ci.CartID == cartId)
                           .Include(ci => ci.Product) // Includi il prodotto per ogni item
                           .ToListAsync();
    }

    // Potresti voler override dei metodi generici se necessiti di includere Product
    // su ogni operazione di GetById o GetAll
    public override async Task<CartItems> GetByIdAsync(int id)
    {
      return await _context.CartItems
                           .Include(ci => ci.Product)
                           .FirstOrDefaultAsync(ci => ci.ID == id);
    }
  }
}
