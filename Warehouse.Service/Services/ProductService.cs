using AutoMapper;
using Warehouse.Common.DTOs;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Services.Services;

namespace Warehouse.Services
{
  public class ProductService : GenericService<Products, DTOProduct>, IProductService
  {
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository productRepository, IMapper mapper, ILogger<ProductService> logger)
        : base(productRepository, mapper, logger)
    {
      _productRepository = productRepository;
      _mapper = mapper;
      _logger = logger;
    }

    public async Task<IEnumerable<DTOProduct>> GetProductsByCategoryAsync(int categoryId)
    {
      _logger.LogInformation("Fetching products by category ID: {CategoryId}", categoryId);
      var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
      if (products == null || !products.Any())
      {
        _logger.LogWarning("No products found for category ID: {CategoryId}", categoryId);
      }
      return _mapper.Map<IEnumerable<DTOProduct>>(products);
    }

    public async Task<DTOProduct?> GetProductDetailsAsync(int id)
    {
      _logger.LogInformation("Fetching product details for Product ID: {ProductId}", id);
      var product = await _productRepository.GetProductDetailsAsync(id);
      if (product == null)
      {
        _logger.LogWarning("Product with ID {ProductId} not found", id);
      }
      return _mapper.Map<DTOProduct?>(product);
    }

    public async Task<IEnumerable<DTOProduct>> GetProductsByOrderIdAsync(int orderId)
    {
      _logger.LogInformation("Fetching products for Order ID: {OrderId}", orderId);
      var products = await _productRepository.GetProductsByOrderIdAsync(orderId);
      return _mapper.Map<IEnumerable<DTOProduct>>(products);
    }
  }
}
