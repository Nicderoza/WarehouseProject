// Warehouse.Interfaces.IServices/ICartItemService.cs
using Warehouse.Common.Responses;
using Warehouse.Common.DTOs;
using System.Threading.Tasks;

namespace Warehouse.Interfaces.IServices
{
  public interface ICartItemService
  {
    Task<BaseResponse<DTOCartItem>> GetCartItemByIdAsync(int cartItemId);
    // Add, Update, Delete per gli item potrebbero essere qui se non gestiti da CartService
    // Task<BaseResponse<DTOCartItem>> AddCartItemAsync(DTOCartAddItemRequest request);
    // Task<BaseResponse<DTOCartItem>> UpdateCartItemAsync(DTOCartUpdateItemQuantityRequest request);
    // Task<BaseResponse<bool>> DeleteCartItemAsync(int cartItemId);
  }
}
