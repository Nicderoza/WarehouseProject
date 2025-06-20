// Warehouse.Service.Services/CartItemService.cs
using AutoMapper;
using Microsoft.Extensions.Logging;
using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using System.Net;

namespace Warehouse.Service.Services
{

  public class CartItemService : GenericService<CartItems, DTOCartItem>, ICartItemService
  {
    private readonly ICartItemRepository _cartItemRepository; 
    private readonly IMapper _mapper;
    private readonly ILogger<CartItemService> _logger;

    public CartItemService(
        ICartItemRepository cartItemRepository,
        IMapper mapper,
        ILogger<CartItemService> logger)
        : base(cartItemRepository, mapper, logger) 
    {
      _cartItemRepository = cartItemRepository;
      _mapper = mapper;
      _logger = logger;
    }

    public async Task<BaseResponse<DTOCartItem>> GetCartItemByIdAsync(int cartItemId)
    {
      _logger.LogInformation($"CartItemService: Recupero articolo carrello con ID {cartItemId}.");
      var cartItem = await _cartItemRepository.GetByIdAsync(cartItemId);
      if (cartItem == null)
      {
        _logger.LogWarning($"CartItemService: Articolo carrello con ID {cartItemId} non trovato.");
        return BaseResponse<DTOCartItem>.NotFoundResponse("Articolo carrello non trovato.", (int)HttpStatusCode.NotFound);
      }
      var cartItemDto = _mapper.Map<DTOCartItem>(cartItem);
      return BaseResponse<DTOCartItem>.SuccessResponse(cartItemDto, "Articolo carrello trovato.", (int)HttpStatusCode.OK);
    }


  }
}
