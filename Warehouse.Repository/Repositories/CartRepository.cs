using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;

namespace Warehouse.Data.Repositories
{
  public class CartRepository : GenericRepository<Cart>, ICartRepository
  {
    public CartRepository(WarehouseContext context) : base(context)
    {
    }

    public async Task<Cart> GetActiveCartByUserIdAsync(int userId)
    {
      return await _context.Carts
          .Include(c => c.CartItems)
              .ThenInclude(ci => ci.Product)
                  .ThenInclude(p => p.Category)
          .Include(c => c.CartItems)
              .ThenInclude(ci => ci.Product)
                  .ThenInclude(p => p.Supplier)
          .FirstOrDefaultAsync(c => c.UserID == userId && c.Status == Warehouse.Data.Models.CartStatus.Active);
    }


    public async Task<CartItems> GetCartItemByCartIdAndProductIdAsync(int cartId, int productId)
    {
      return await _context.CartItems
          .Include(ci => ci.Product)
              .ThenInclude(p => p.Category)
          .Include(ci => ci.Product)
              .ThenInclude(p => p.Supplier)
          .FirstOrDefaultAsync(ci => ci.CartID == cartId && ci.ProductID == productId);
    }

    public async Task<CartItems> GetCartItemByIdAsync(int cartItemId)
    {
      return await _context.CartItems
          .Include(ci => ci.Product)
              .ThenInclude(p => p.Category)
          .Include(ci => ci.Product)
              .ThenInclude(p => p.Supplier)
          .FirstOrDefaultAsync(ci => ci.ID == cartItemId);
    }

    public async Task AddCartItemAsync(CartItems cartItem)
    {
      cartItem.CreatedAt = DateTime.Now;
      cartItem.UpdatedAt = DateTime.Now;
      _context.CartItems.Add(cartItem);
    }

    public async Task UpdateCartItemAsync(CartItems cartItem)
    {
      cartItem.UpdatedAt = DateTime.Now;
      _context.CartItems.Update(cartItem);
    }

    public async Task DeleteCartItemAsync(CartItems cartItem)
    {
      _context.CartItems.Remove(cartItem);
    }

  }
}
