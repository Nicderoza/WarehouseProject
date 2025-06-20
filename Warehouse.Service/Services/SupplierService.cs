using AutoMapper;
using Microsoft.Extensions.Logging;
using Warehouse.Common.DTOs;
using Warehouse.Common.Responses; 
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using System.Linq;
using System;

namespace Warehouse.Service.Services
{
  using Resp = Warehouse.Common.Responses; 

  public class SupplierService : GenericService<Suppliers, DTOSupplier>, ISupplierService
  {
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GenericService<Suppliers, DTOSupplier>> _logger;

    public SupplierService(ISupplierRepository supplierRepository, IMapper mapper, ILogger<GenericService<Suppliers, DTOSupplier>> logger)
        : base(supplierRepository, mapper, logger)
    {
      _supplierRepository = supplierRepository;
      _mapper = mapper;
      _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<DTOSupplier>>> GetSuppliersByCityAsync(int cityId)
    {
      try
      {
        var suppliers = await _supplierRepository.GetSuppliersByCityAsync(cityId);

        if (suppliers == null || !suppliers.Any())
        {
          return BaseResponse<IEnumerable<DTOSupplier>>.NotFoundResponse($"Nessun fornitore trovato per la città con ID {cityId}.");
        }

        var dtos = _mapper.Map<IEnumerable<DTOSupplier>>(suppliers);
        return BaseResponse<IEnumerable<DTOSupplier>>.SuccessResponse(dtos, $"Fornitori nella città con ID {cityId} recuperati con successo.");
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Errore durante il recupero dei fornitori per la città con ID {cityId}.");
        return BaseResponse<IEnumerable<DTOSupplier>>.ErrorResponse($"Errore interno del server durante il recupero dei fornitori: {ex.Message}", 500);
      }
    }

    public async Task<BaseResponse<DTOSupplier>> CreateSupplierProfileAsync(int userId, string companyName, int cityId)
    {
      if (string.IsNullOrWhiteSpace(companyName))
      {
        return BaseResponse<DTOSupplier>.BadRequestResponse("Il nome della compagnia non può essere vuoto.");
      }

      if (cityId <= 0)
      {
        return BaseResponse<DTOSupplier>.BadRequestResponse("L'ID della città non è valido.");
      }

      try
      {
        var existingSupplier = await _supplierRepository.GetByNameAsync(companyName);
        if (existingSupplier != null)
        {
          string errorMessage = ErrorControl.GetErrorMessage(Resp.ErrorType.ProfileAlreadyExists);
          return BaseResponse<DTOSupplier>.ConflictResponse(errorMessage);
        }

        var newSupplier = new Suppliers
        {
          CompanyName = companyName,
          CityID = cityId,
          CreatedAt = DateTime.UtcNow,
          IsActive = true
        };

        var createdSupplier = await _supplierRepository.AddAsync(newSupplier);
        await _supplierRepository.SaveChangesAsync();

        var createdSupplierDto = _mapper.Map<DTOSupplier>(createdSupplier);

        _logger.LogInformation($"Fornitore '{companyName}' (ID: {createdSupplier.SupplierID}) creato con successo dall'utente {userId}.");
        return BaseResponse<DTOSupplier>.SuccessResponse(createdSupplierDto, $"Profilo fornitore '{companyName}' creato con successo.");
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Errore durante la creazione del profilo fornitore per '{companyName}' da parte dell'utente {userId}.");
        return BaseResponse<DTOSupplier>.ErrorResponse($"Errore interno del server durante la creazione del profilo fornitore: {ex.Message}", 500);
      }
    }
      public async Task<DTOSupplier?> GetByNameAsync(string companyName)
    {
      var supplier = await _supplierRepository.GetByNameAsync(companyName);
      return supplier != null ? _mapper.Map<DTOSupplier>(supplier) : null;
    }
    public async Task<List<DTOProduct>> GetProductsByUserIdAsync(int userId)
    {
      var supplierId = await _supplierRepository.GetSupplierIdByUserIdAsync(userId);
      if (supplierId == null)
        throw new Exception("Supplier not found for this user");

      var products = await _supplierRepository.GetProductsBySupplierIdAsync(supplierId.Value);
      return _mapper.Map<List<DTOProduct>>(products);
    }

    public async Task<DTOSupplier?> GetSupplierByUserIdAsync(int userId)
    {
      var supplier = await _supplierRepository.GetSupplierByUserIdAsync(userId);

      if (supplier == null)
      {
        Console.WriteLine($"⚠️ Nessun fornitore trovato per UserId {userId}");
        return null;
      }

      try
      {
        return _mapper.Map<DTOSupplier>(supplier);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"❌ Errore nella mappatura AutoMapper: {ex.Message}");
        throw;
      }
    }


  }
}

