// Warehouse.Services.Services/OrderService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using Warehouse.Service.Services;

namespace Warehouse.Services.Services
{
  public class OrderService : GenericService<Orders, DTOOrder>, IOrderService
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IGenericRepository<OrderStatus> _orderStatusRepository;
    private readonly ILogger<OrderService> _logger;
    private readonly ICartRepository _cartRepository;
    private readonly ICartService _cartService;
    private readonly WarehouseContext _context;


    public OrderService(
  IOrderRepository orderRepository,
  IProductRepository productRepository,
  IMapper mapper,
  ILogger<OrderService> logger,
  IGenericRepository<OrderStatus> orderStatusRepository,
  ICartRepository cartRepository,
  ICartService cartService,
  WarehouseContext context 

) : base(orderRepository, mapper, logger)
    {
      _orderRepository = orderRepository;
      _productRepository = productRepository;
      _orderStatusRepository = orderStatusRepository;
      _logger = logger;
      _cartRepository = cartRepository;
      _cartService = cartService;
      _context = context;
    }

    public async Task<BaseResponse<DTOOrder>> CreateOrderAsync(DTOOrderCreateRequest createOrderDto)
    {
      if (createOrderDto == null || !createOrderDto.Items.Any())
      {
        return BaseResponse<DTOOrder>.BadRequestResponse("L'ordine non può essere vuoto o nullo.");
      }

      var productIds = createOrderDto.Items.Select(i => i.ProductID).Distinct().ToList();
      _logger.LogInformation("CreateOrder: Ricevuti product IDs: {Ids}", string.Join(", ", productIds));

      var products = await _productRepository.GetByIdsAsync(productIds);

      if (products == null || !products.Any())
      {
        return BaseResponse<DTOOrder>.NotFoundResponse("Nessun prodotto trovato per gli ID specificati.");
      }

      var orderItems = new List<OrderItems>();
      decimal totalAmount = 0;

      foreach (var item in createOrderDto.Items)
      {
        var product = products.FirstOrDefault(p => p.ProductID == item.ProductID);
        if (product == null)
        {
          return BaseResponse<DTOOrder>.NotFoundResponse($"Prodotto con ID {item.ProductID} non trovato.");
        }

        if (product.Quantity < item.Quantity)
        {
          return BaseResponse<DTOOrder>.BadRequestResponse(
              $"Quantità insufficiente per il prodotto '{product.Name}'. Disponibili: {product.Quantity}"
          );
        }

        orderItems.Add(new OrderItems
        {
          ProductID = product.ProductID,
          Quantity = item.Quantity,
          UnitPrice = product.Price,
          Price = product.Price * item.Quantity,
          ProductName = product.Name,
          CategoryName = product.Category?.CategoryName,
          SupplierName = product.Supplier?.CompanyName,
          CreatedAt = DateTime.UtcNow,
          UpdatedAt = DateTime.UtcNow
        });



        totalAmount += product.Price * item.Quantity;
      }

      var status = await _orderStatusRepository.FirstOrDefaultAsync(s => s.StatusName == "In Lavorazione");
      if (status == null)
      {
        return BaseResponse<DTOOrder>.ErrorResponse("Stato 'In Lavorazione' non configurato nel database.", 500);
      }

      var newOrder = new Orders
      {
        UserID = createOrderDto.UserID,
        OrderDate = DateTime.UtcNow,
        TotalAmount = totalAmount,
        OrderStatusFkID = status.OrderStatusID,
        OrderItems = orderItems
      };

      try
      {
        await _orderRepository.BeginTransactionAsync();

        foreach (var item in createOrderDto.Items)
        {
          var product = products.FirstOrDefault(p => p.ProductID == item.ProductID);
          if (product == null)
          {
            await _orderRepository.RollbackTransactionAsync();
            return BaseResponse<DTOOrder>.NotFoundResponse($"Prodotto con ID {item.ProductID} non trovato durante l'aggiornamento.");
          }

          product.Quantity -= item.Quantity;
          await _productRepository.UpdateAsync(product);
        }

        await _orderRepository.AddAsync(newOrder);
        await _orderRepository.SaveChangesAsync();
        await _orderRepository.CommitTransactionAsync();

        var dtoOrder = _mapper.Map<DTOOrder>(newOrder);
        return BaseResponse<DTOOrder>.SuccessResponse(dtoOrder, "Ordine creato con successo.", 201);
      }
      catch (Exception ex)
      {
        await _orderRepository.RollbackTransactionAsync();
        return BaseResponse<DTOOrder>.ErrorResponse($"Errore durante la creazione dell'ordine: {ex.Message}", 500);
      }

    }


    public async Task<BaseResponse<DTOOrder>> ChangeOrderStatusAsync(int orderId, string newStatusName)
    {
      var order = await _orderRepository.GetOrderByIdAsync(orderId);
      if (order == null)
      {
        return BaseResponse<DTOOrder>.NotFoundResponse("Ordine non trovato.");
      }

      var newStatus = await _orderStatusRepository.FirstOrDefaultAsync(s => s.StatusName == newStatusName);
      if (newStatus == null)
      {
        return BaseResponse<DTOOrder>.BadRequestResponse($"Stato '{newStatusName}' non valido.");
      }

      order.OrderStatusFkID = newStatus.OrderStatusID;
      await _orderRepository.UpdateAsync(order);
      await _orderRepository.SaveChangesAsync();

      var dto = _mapper.Map<DTOOrder>(order);
      return BaseResponse<DTOOrder>.SuccessResponse(dto, "Stato ordine aggiornato con successo.");
    }

    public async Task<BaseResponse<IEnumerable<DTOOrder>>> GetOrdersByUserIdAsync(int userId)
    {
      var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
      if (!orders.Any())
      {
        return BaseResponse<IEnumerable<DTOOrder>>.NotFoundResponse("Nessun ordine trovato per l'utente.");
      }

      var dtos = _mapper.Map<IEnumerable<DTOOrder>>(orders);
      return BaseResponse<IEnumerable<DTOOrder>>.SuccessResponse(dtos);
    }


    public async Task<BaseResponse<IEnumerable<DTOOrder>>> GetOrdersBySupplierAsync(int  supplierId)
    {
      var orders = await _orderRepository.GetOrdersBySupplierAsync(supplierId);
      if (!orders.Any())
      {
        return BaseResponse<IEnumerable<DTOOrder>>.NotFoundResponse($"Nessun ordine per il fornitore '{supplierId}'.");
      }

      var dtos = _mapper.Map<IEnumerable<DTOOrder>>(orders);
      return BaseResponse<IEnumerable<DTOOrder>>.SuccessResponse(dtos);
    }

    public async Task<BaseResponse<IEnumerable<DTOOrder>>> GetOrdersByCategoryAsync(string categoryName)
    {
      var orders = await _orderRepository.GetOrdersByCategoryAsync(categoryName);
      if (!orders.Any())
      {
        return BaseResponse<IEnumerable<DTOOrder>>.NotFoundResponse($"Nessun ordine per la categoria '{categoryName}'.");
      }

      var dtos = _mapper.Map<IEnumerable<DTOOrder>>(orders);
      return BaseResponse<IEnumerable<DTOOrder>>.SuccessResponse(dtos);
    }

    public async Task<BaseResponse<DTOOrder>> GetOrderDetailsAsync(int orderId)
    {
      var order = await _orderRepository.GetOrderByIdAsync(orderId);
      if (order == null)
      {
        return BaseResponse<DTOOrder>.NotFoundResponse("Dettagli ordine non trovati.");
      }

      var dto = _mapper.Map<DTOOrder>(order);
      return BaseResponse<DTOOrder>.SuccessResponse(dto);
    }

    public async Task<BaseResponse<DTOOrder>> CheckoutFromCartAsync(int userId)
    {
      var user = await _context.Users
          .Include(u => u.Cart)
          .ThenInclude(c => c.CartItems)
          .ThenInclude(ci => ci.Product)
          .FirstOrDefaultAsync(u => u.UserID == userId);

      if (user == null || user.Cart == null || user.Cart.CartItems == null || !user.Cart.CartItems.Any())
      {
        return new BaseResponse<DTOOrder>
        {
          Success = false,
          Message = "Carrello vuoto o utente non trovato."
        };
      }

      var totalAmount = user.Cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);

      var orderItems = user.Cart.CartItems.Select(ci => new OrderItems
      {
        ProductID = ci.ProductID,
        Quantity = ci.Quantity,
        UnitPrice = ci.Product.Price,
        Price = ci.Product.Price * ci.Quantity,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
      }).ToList();

      var order = new Orders
      {
        UserID = userId,
        OrderDate = DateTime.UtcNow,
        TotalAmount = totalAmount,
        OrderStatusFkID = 1, 
        OrderItems = orderItems
      };

      _context.Orders.Add(order);

      _context.CartItems.RemoveRange(user.Cart.CartItems);

      await _context.SaveChangesAsync();

      var statusName = await _context.OrderStatuses
    .Where(os => os.OrderStatusID == order.OrderStatusFkID)
    .Select(os => os.StatusName)
    .FirstOrDefaultAsync();

      _logger.LogInformation("StatusName retrieved: {StatusName}", statusName ?? "NULL");

      if (statusName == null)
      {
        statusName = "Unknown"; // fallback per sicurezza
      }

      var dtoOrder = new DTOOrder
      {
        OrderID = order.OrderID,
        UserID = userId,
        OrderDate = order.OrderDate,
        TotalAmount = order.TotalAmount,
        OrderStatusID = order.OrderStatusFkID,
        StatusName = statusName,
        OrderItems = order.OrderItems.Select(oi => new DTOOrderItem
        {
          OrderItemID = oi.OrderItemID,
          OrderID = oi.OrderID,
          ProductID = oi.ProductID,
          Quantity = oi.Quantity,
          UnitPrice = oi.UnitPrice,
          Price = oi.Price,
          ProductName = oi.Product?.Name,
          CategoryName = oi.Product?.Category?.CategoryName,
          SupplierName = oi.Product?.Supplier?.CompanyName
        }).ToList()
      };

      return new BaseResponse<DTOOrder>
      {
        Success = true,
        Data = dtoOrder,
        Message = "Ordine effettuato con successo."
      };
    }
  }
 }
