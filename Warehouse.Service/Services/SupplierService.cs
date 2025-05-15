using AutoMapper;
using Microsoft.Extensions.Logging; // Assicurati che questo namespace sia presente
using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using Warehouse.Services.Services;

namespace Warehouse.Service.Services
{
    public class SupplierService : GenericService<Suppliers, DTOSupplier>, ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        // Modifica il costruttore per accettare e passare il logger
        public SupplierService(ISupplierRepository supplierRepository, IMapper mapper, ILogger<GenericService<Suppliers, DTOSupplier>> logger)
            : base(supplierRepository, mapper, logger)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<IEnumerable<DTOSupplier>>> GetSuppliersByCityAsync(int cityId)
        {
            var suppliers = await _supplierRepository.GetSuppliersByCityAsync(cityId);
            var dtos = _mapper.Map<IEnumerable<DTOSupplier>>(suppliers);
            return BaseResponse<IEnumerable<DTOSupplier>>.SuccessResponse(dtos, $"Fornitori nella città con ID {cityId} recuperati con successo.");
        }
    }
}