using Warehouse.Common.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Common.DTOs;
using Warehouse.Interfaces.IServices;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Data.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Warehouse.Services.Services
{
  public class OrderService : GenericService<Orders, DTOOrder>, IOrderService
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository, IMapper mapper, ILogger<OrderService> logger, IProductRepository productRepository) : base(orderRepository, mapper, logger)
    {
      _orderRepository = orderRepository;
      _mapper = mapper;
      _logger = logger;
      _productRepository = productRepository;
    }

    public async Task<BaseResponse<IEnumerable<DTOOrder>>> GetAllAsync()
    {
      var orders = await _orderRepository.GetAllAsync();
      var dtoOrders = _mapper.Map<IEnumerable<DTOOrder>>(orders);
      return BaseResponse<IEnumerable<DTOOrder>>.SuccessResponse(dtoOrders);
    }

    public async Task<BaseResponse<DTOOrder>> GetByIdAsync(int id)
    {
      var order = await _orderRepository.GetByIdAsync(id);
      if (order == null)
      {
        return BaseResponse<DTOOrder>.NotFoundResponse($"Ordine con ID {id} non trovato.");
      }
      var dtoOrder = _mapper.Map<DTOOrder>(order);
      return BaseResponse<DTOOrder>.SuccessResponse(dtoOrder);
    }
public async Task<BaseResponse<IEnumerable<DTOOrder>>> GetOrdersByUserIdAsync(int userId)
    {
      var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
      var dtoOrders = _mapper.Map<IEnumerable<DTOOrder>>(orders);
      return BaseResponse<IEnumerable<DTOOrder>>.SuccessResponse(dtoOrders);
    }

    public async Task<BaseResponse<IEnumerable<DTOOrder>>> GetOrdersBySupplierAsync(string supplierName)
    {
      var orders = await _orderRepository.GetOrdersBySupplierAsync(supplierName);
      var dtoOrders = _mapper.Map<IEnumerable<DTOOrder>>(orders);
      return BaseResponse<IEnumerable<DTOOrder>>.SuccessResponse(dtoOrders);
    }

    public async Task<BaseResponse<IEnumerable<DTOOrder>>> GetOrdersByCategoryAsync(string categoryName)
    {
      var orders = await _orderRepository.GetOrdersByCategoryAsync(categoryName);
      var dtoOrders = _mapper.Map<IEnumerable<DTOOrder>>(orders);
      return BaseResponse<IEnumerable<DTOOrder>>.SuccessResponse(dtoOrders);
    }

    public async Task<BaseResponse<DTOOrder>> ChangeOrderStatusAsync(int orderId, string newStatus)
    {
      var order = await _orderRepository.GetOrderByIdAsync(orderId);

      if (order == null)
      {
        return BaseResponse<DTOOrder>.NotFoundResponse($"Ordine con ID {orderId} non trovato.");
      }

      order.Status = newStatus;
      await _orderRepository.UpdateOrderAsync(order);

      if (newStatus.ToLower() == "spedito")
      {
        await UpdateProductQuantitiesAsync(orderId);
      }

      var dtoOrder = _mapper.Map<DTOOrder>(order);
      return BaseResponse<DTOOrder>.SuccessResponse(dtoOrder, $"Stato dell'ordine {orderId} aggiornato a {newStatus}.");
    }
    private async Task UpdateProductQuantitiesAsync(int orderId)
    {
      var order = await _orderRepository.GetOrderByIdAsync(orderId);
      if (order == null)
      {
        _logger.LogError($"Ordine con ID {orderId} non trovato.");
        return;
      }

      if (order.OrderItems == null || !order.OrderItems.Any())
      {
        _logger.LogWarning($"Nessun elemento dell'ordine trovato per l'ordine con ID {orderId}.");
        return;
      }

      foreach (var item in order.OrderItems)
      {
        var product = await _productRepository.GetByIdAsync(item.ProductID);
        if (product != null)
        {
          if (product.Quantity >= item.Quantity)
          {
            product.Quantity -= item.Quantity;
            await _productRepository.UpdateProductAsync(product);
            _logger.LogInformation($"Quantità del prodotto {product.Name} (ID: {product.ProductID}) aggiornata di {item.Quantity}.");
          }
          else
          {
            _logger.LogError($"Quantità insufficiente in magazzino per il prodotto {product.Name} (ID: {product.ProductID}).");
          }
        }
        else
        {
          _logger.LogError($"Prodotto con ID {item.ProductID} non trovato.");
        }
      }
    }

    public async Task<BaseResponse<DTOOrder>> GetOrderDetailsAsync(int orderId)
    {
      // 1. Trova l'ordine usando l'ID tramite il repository
      var order = await _orderRepository.GetOrderByIdAsync(orderId);

      // 2. Controlla se l'ordine è stato trovato
      if (order == null)
      {
        return BaseResponse<DTOOrder>.NotFoundResponse($"Ordine con ID {orderId} non trovato.");
      }

      // 3. Se l'ordine è stato trovato, lo trasformiamo in un DTO
      var dtoOrder = _mapper.Map<DTOOrder>(order);

      // 4. Restituiamo una risposta di successo con i dettagli dell'ordine
      return BaseResponse<DTOOrder>.SuccessResponse(dtoOrder);
    }
  }
}
