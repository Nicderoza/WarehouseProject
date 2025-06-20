using System.Net;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;

namespace Warehouse.Service.Services
{
  public class CartService : GenericService<Cart, DTOCartResponse>, ICartService
  {
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CartService> _logger;

    public CartService(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<CartService> logger)
        : base(cartRepository, mapper, logger)
    {
      _cartRepository = cartRepository;
      _productRepository = productRepository;
      _mapper = mapper;
      _logger = logger;
    }

    public async Task<BaseResponse<DTOCartResponse>> GetCartByUserIdAsync(int userId)
    {
      _logger.LogInformation($"[GetCartByUserIdAsync] Recupero carrello per utente {userId}");
      var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId);

      if (cart == null)
      {
        cart = new Cart
        {
          UserID = userId,
          Status = CartStatus.Active,
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        };

        await _cartRepository.AddAsync(cart);
        await _cartRepository.SaveChangesAsync();
      }

      var cartDto = _mapper.Map<DTOCartResponse>(cart);
      return BaseResponse<DTOCartResponse>.SuccessResponse(cartDto, "Carrello recuperato con successo.", (int)HttpStatusCode.OK);
    }

    public async Task<BaseResponse<DTOCartResponse>> AddItemToCartAsync(int userId, DTOAddItemCartRequest requestDto)
    {
      _logger.LogInformation($"[AddItemToCartAsync] Aggiunta articolo {requestDto.ProductID} a utente {userId}");

      var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId);
      if (cart == null)
        return BaseResponse<DTOCartResponse>.ErrorResponse("Carrello non disponibile.", (int)HttpStatusCode.InternalServerError);

      var product = await _productRepository.GetByIdAsync(requestDto.ProductID);
      if (product == null)
        return BaseResponse<DTOCartResponse>.NotFoundResponse("Prodotto non trovato.", (int)HttpStatusCode.NotFound);

      if (product.Quantity < requestDto.Quantity)
        return BaseResponse<DTOCartResponse>.ErrorResponse("Quantità richiesta non disponibile.", (int)HttpStatusCode.BadRequest);

      var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductID == requestDto.ProductID);

      if (existingItem != null)
      {
        existingItem.Quantity += requestDto.Quantity;
        existingItem.UpdatedAt = DateTime.UtcNow;
      }
      else
      {
        cart.CartItems.Add(new CartItems
        {
          ProductID = requestDto.ProductID,
          Quantity = requestDto.Quantity,
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        });
      }

      await _cartRepository.UpdateAsync(cart);
      await _cartRepository.SaveChangesAsync();

      var updatedCart = await _cartRepository.GetActiveCartByUserIdAsync(userId);
      var cartDto = _mapper.Map<DTOCartResponse>(updatedCart);
      return BaseResponse<DTOCartResponse>.SuccessResponse(cartDto, "Articolo aggiunto al carrello.", (int)HttpStatusCode.OK);
    }

    public async Task<BaseResponse<DTOCartResponse>> UpdateCartItemQuantityAsync(int userId, DTOCartUpdateItemQuantityRequest requestDto)
    {
      _logger.LogInformation($"[UpdateCartItemQuantityAsync] Modifica quantità articolo {requestDto.CartItemID} per utente {userId}");

      var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId);
      if (cart == null)
        return BaseResponse<DTOCartResponse>.NotFoundResponse("Carrello non trovato.", (int)HttpStatusCode.NotFound);

      var item = cart.CartItems.FirstOrDefault(i => i.ID == requestDto.CartItemID);
      if (item == null)
        return BaseResponse<DTOCartResponse>.NotFoundResponse("Articolo del carrello non trovato.", (int)HttpStatusCode.NotFound);

      if (requestDto.Quantity <= 0)
        cart.CartItems.Remove(item);
      else
      {
        item.Quantity = requestDto.Quantity;
        item.UpdatedAt = DateTime.UtcNow;
      }

      await _cartRepository.UpdateAsync(cart);
      await _cartRepository.SaveChangesAsync();

      var updatedCart = await _cartRepository.GetActiveCartByUserIdAsync(userId);
      var cartDto = _mapper.Map<DTOCartResponse>(updatedCart);
      return BaseResponse<DTOCartResponse>.SuccessResponse(cartDto, "Quantità aggiornata.", (int)HttpStatusCode.OK);
    }

    public async Task<BaseResponse<DTOCartResponse>> RemoveItemFromCartAsync(int userId, int cartItemId)
    {
      _logger.LogInformation($"[RemoveItemFromCartAsync] Rimozione articolo {cartItemId} da carrello utente {userId}");

      var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId);
      if (cart == null)
        return BaseResponse<DTOCartResponse>.NotFoundResponse("Carrello non trovato.", (int)HttpStatusCode.NotFound);

      var item = cart.CartItems.FirstOrDefault(i => i.ID == cartItemId);
      if (item == null)
        return BaseResponse<DTOCartResponse>.NotFoundResponse("Articolo non trovato.", (int)HttpStatusCode.NotFound);

      cart.CartItems.Remove(item);

      await _cartRepository.UpdateAsync(cart);
      await _cartRepository.SaveChangesAsync();

      var updatedCart = await _cartRepository.GetActiveCartByUserIdAsync(userId);
      var cartDto = _mapper.Map<DTOCartResponse>(updatedCart);
      return BaseResponse<DTOCartResponse>.SuccessResponse(cartDto, "Articolo rimosso con successo.", (int)HttpStatusCode.OK);
    }

    // ✅ ClearCartAsync
    public async Task<BaseResponse<bool>> ClearCartAsync(int userId)
    {
      _logger.LogInformation($"[ClearCartAsync] Pulizia carrello utente {userId}");

      var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId);
      if (cart == null)
        return BaseResponse<bool>.NotFoundResponse("Carrello non trovato.", (int)HttpStatusCode.NotFound);

      cart.CartItems.Clear();
      await _cartRepository.UpdateAsync(cart);
      await _cartRepository.SaveChangesAsync();

      return BaseResponse<bool>.SuccessResponse(true, "Carrello svuotato.", (int)HttpStatusCode.OK);
    }
  }
}
