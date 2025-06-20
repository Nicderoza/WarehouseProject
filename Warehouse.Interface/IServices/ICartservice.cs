// Warehouse.Interfaces.IServices/ICartService.cs
using Warehouse.Common.Responses;
using Warehouse.Common.DTOs;
using Warehouse.Data.Models; // Necessario per il tipo CartEntity se vuoi essere specifico

namespace Warehouse.Interfaces.IServices
{
  // Ora ICartService estende IGenericService per il tipo Cart e DTOCartResponse
  public interface ICartService : IGenericService< DTOCartResponse>
  {
    // Metodi specifici del carrello, oltre a quelli ereditati da IGenericService
    Task<BaseResponse<DTOCartResponse>> GetCartByUserIdAsync(int userId);
    Task<BaseResponse<DTOCartResponse>> AddItemToCartAsync(int userId, DTOAddItemCartRequest requestDto);
    Task<BaseResponse<DTOCartResponse>> UpdateCartItemQuantityAsync(int userId, DTOCartUpdateItemQuantityRequest requestDto);
    Task<BaseResponse<DTOCartResponse>> RemoveItemFromCartAsync(int userId, int cartItemId);
    Task<BaseResponse<bool>> ClearCartAsync(int userId);

  }
}
