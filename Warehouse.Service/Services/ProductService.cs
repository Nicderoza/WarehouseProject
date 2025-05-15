using AutoMapper;
using Warehouse.Common.DTOs;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Warehouse.Services.Services;  // Ensure you are using this for logging

namespace Warehouse.Services
{
    public class ProductService : GenericService<Products, DTOProduct>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        // Modify the constructor to include logger
        public ProductService(IProductRepository productRepository, IMapper mapper, ILogger<ProductService> logger)
            : base(productRepository, mapper, logger) // Pass the logger to the base constructor
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;  // Assign it to a field if you need to use it in this class
        }

        public async Task<IEnumerable<DTOProduct>> GetProductsByCategoryAsync(int categoryId)
        {
            _logger.LogInformation("Fetching products by category ID: {CategoryId}", categoryId); // Log the category ID being queried
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
            if (products == null || !products.Any())
            {
                _logger.LogWarning("No products found for category ID: {CategoryId}", categoryId);
            }
            return _mapper.Map<IEnumerable<DTOProduct>>(products);
        }

        public async Task<DTOProduct?> GetProductDetailsAsync(int id)
        {
            _logger.LogInformation("Fetching product details for Product ID: {ProductId}", id); // Log the product ID being fetched
            var product = await _productRepository.GetProductDetailsAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", id); // Log if product is not found
            }
            return _mapper.Map<DTOProduct?>(product);
        }

        public async Task<IEnumerable<DTOProduct>> GetProductsByOrderIdAsync(int orderId)
        {
            _logger.LogInformation("Fetching products for Order ID: {OrderId}", orderId); // Log the order ID
            var products = await _productRepository.GetProductsByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<DTOProduct>>(products);
        }
    }
}
