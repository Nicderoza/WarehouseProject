using System.Net; // Per HttpStatusCode
using Microsoft.AspNetCore.Mvc;
using Warehouse.Common.DTOs; // Assicurati di avere i tuoi DTO qui
using Warehouse.Common.Responses; // Assicurati di avere BaseResponse qui
using Warehouse.Interfaces.IServices; // Per ICartService

namespace Warehouse.API.Controllers
{
  [ApiController] // Indica che è un controller API
  [Route("api/[controller]")] // Definisce il percorso base per il controller (es. /api/Cart)
  public class CartsController : ControllerBase
  {
    private readonly ICartService _cartService;
    private readonly ILogger<CartsController> _logger;
    private readonly IOrderService _orderService;


    public CartsController(ICartService cartService, ILogger<CartsController> logger, IOrderService orderService)
    {
      _cartService = cartService;
      _logger = logger;
      _orderService = orderService;
    }

    // GET: api/Cart/user/{userId}
    /// <summary>
    /// Recupera il carrello attivo di un utente specifico. Se non esiste, ne crea uno nuovo.
    /// </summary>
    /// <param name="userId">ID dell'utente.</param>
    /// <returns>Il carrello attivo dell'utente.</returns>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(BaseResponse<DTOCartResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<DTOCartResponse>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetCartByUserId(int userId)
    {
      _logger.LogInformation($"CartController: Richiesta GET carrello per utente {userId}.");
      var response = await _cartService.GetCartByUserIdAsync(userId);

      if (response.Success)
      {
        return Ok(response);
      }
      // Gestione generica degli errori: il servizio dovrebbe già impostare il codice di stato
      return StatusCode(response.StatusCode ?? (int)HttpStatusCode.InternalServerError, response);
    }

    // POST: api/Cart/{userId}/add-item
    /// <summary>
    /// Aggiunge un prodotto al carrello di un utente o aggiorna la quantità se già presente.
    /// </summary>
    /// <param name="userId">ID dell'utente proprietario del carrello.</param>
    /// <param name="requestDto">Dettagli dell'articolo da aggiungere (ID prodotto e quantità).</param>
    /// <returns>Il carrello aggiornato.</returns>
    [HttpPost("{userId}/add-item")]
    [ProducesResponseType(typeof(BaseResponse<DTOCartResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<DTOCartResponse>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<DTOCartResponse>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<DTOCartResponse>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> AddItemToCart(int userId, [FromBody] DTOAddItemCartRequest requestDto)
    {
      _logger.LogInformation($"CartController: Richiesta POST aggiungi articolo {requestDto.ProductID} (Qty: {requestDto.Quantity}) al carrello utente {userId}.");
      var response = await _cartService.AddItemToCartAsync(userId, requestDto);

      // Mapping delle risposte del servizio ai codici di stato HTTP
      if (response.Success)
      {
        return Ok(response);
      }
      if (response.NotFound)
      {
        return NotFound(response);
      }
      // Il servizio ha già un messaggio specifico per BadRequest
      if (response.StatusCode == (int)HttpStatusCode.BadRequest)
      {
        return BadRequest(response);
      }

      return StatusCode(response.StatusCode ?? (int)HttpStatusCode.InternalServerError, response);
    }

    // PUT: api/Cart/{userId}/update-item-quantity
    /// <summary>
    /// Aggiorna la quantità di un articolo specifico nel carrello di un utente.
    /// Se la quantità è 0 o meno, l'articolo viene rimosso.
    /// </summary>
    /// <param name="userId">ID dell'utente proprietario del carrello.</param>
    /// <param name="requestDto">Dettagli dell'articolo da aggiornare (ID articolo carrello e nuova quantità).</param>
    /// <returns>Il carrello aggiornato.</returns>
    [HttpPut("{userId}/update-item-quantity")]
    [ProducesResponseType(typeof(BaseResponse<DTOCartResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<DTOCartResponse>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<DTOCartResponse>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<DTOCartResponse>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> UpdateCartItemQuantity(int userId, [FromBody] DTOCartUpdateItemQuantityRequest requestDto)
    {
      _logger.LogInformation($"CartController: Richiesta PUT aggiorna quantità articolo {requestDto.CartItemID} a {requestDto.Quantity} per utente {userId}.");
      var response = await _cartService.UpdateCartItemQuantityAsync(userId, requestDto);

      if (response.Success)
      {
        return Ok(response);
      }
      if (response.NotFound)
      {
        return NotFound(response);
      }
      if (response.StatusCode == (int)HttpStatusCode.BadRequest)
      {
        return BadRequest(response);
      }

      return StatusCode(response.StatusCode ?? (int)HttpStatusCode.InternalServerError, response);
    }

    // DELETE: api/Cart/{userId}/remove-item/{cartItemId}
    /// <summary>
    /// Rimuove un articolo specifico dal carrello di un utente.
    /// </summary>
    /// <param name="userId">ID dell'utente proprietario del carrello.</param>
    /// <param name="cartItemId">ID dell'articolo del carrello da rimuovere.</param>
    /// <returns>Il carrello aggiornato.</returns>
    [HttpDelete("{userId}/remove-item/{cartItemId}")]
    [ProducesResponseType(typeof(BaseResponse<DTOCartResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<DTOCartResponse>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<DTOCartResponse>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> RemoveItemFromCart(int userId, int cartItemId)
    {
      _logger.LogInformation($"CartController: Richiesta DELETE rimuovi articolo {cartItemId} dal carrello utente {userId}.");
      var response = await _cartService.RemoveItemFromCartAsync(userId, cartItemId);

      if (response.Success)
      {
        return Ok(response);
      }
      if (response.NotFound)
      {
        return NotFound(response);
      }

      return StatusCode(response.StatusCode ?? (int)HttpStatusCode.InternalServerError, response);
    }
    [HttpPost("{userId}/checkout")]
    [ProducesResponseType(typeof(BaseResponse<object>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> CheckoutCart(int userId)
    {
      _logger.LogInformation($"CartController: Checkout per utente {userId} iniziato.");

      var cartResponse = await _cartService.GetCartByUserIdAsync(userId);
      if (!cartResponse.Success || cartResponse.Data == null || cartResponse.Data.Items == null || !cartResponse.Data.Items.Any())
      {
        _logger.LogWarning($"Carrello per utente {userId} non trovato o vuoto.");
        return BadRequest("Il carrello è vuoto o inesistente.");
      }

      var cart = cartResponse.Data;

      var orderRequest = new DTOOrderCreateRequest
      {
        UserID = userId,
        Items = cart.Items.Select(i => new DTOOrderItemRequest
        {
          ProductID = i.ProductID,
          Quantity = i.Quantity,
          UnitPrice = i.UnitPrice
        }).ToList(),
        TotalAmount = cart.Items.Sum(i => i.Quantity * i.UnitPrice)
      };

      var orderResponse = await _orderService.CreateOrderAsync(orderRequest);
      if (!orderResponse.Success)
      {
        _logger.LogError("Errore nella creazione dell'ordine: {Message}", orderResponse.Message);
        return StatusCode(orderResponse.StatusCode ?? 500, orderResponse.Message);
      }

      // (Opzionale) svuota il carrello
      var clearResponse = await _cartService.ClearCartAsync(userId);
      if (!clearResponse.Success)
      {
        _logger.LogWarning($"Ordine creato ma impossibile svuotare il carrello per utente {userId}.");
      }

      _logger.LogInformation($"Ordine creato con successo per utente {userId}.");
      return Ok(orderResponse.Data);
    }

  }
}
