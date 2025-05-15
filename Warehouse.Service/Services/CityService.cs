using AutoMapper;
using Microsoft.Extensions.Logging;
using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using Warehouse.Services.Services;

namespace Warehouse.Service.Services
{
  public class CityService : GenericService<Cities, DTOCity>, ICityService
  {
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CityService> _logger;

    public CityService(ICityRepository cityRepository, IMapper mapper, ILogger<CityService> logger)
        : base(cityRepository, mapper, logger)
    {
      _cityRepository = cityRepository;
      _mapper = mapper;
      _logger = logger;
    }
    public override async Task<BaseResponse<IEnumerable<DTOCity>>> GetAllAsync()
    {
      var entities = await _cityRepository.GetAllAsync();
      var dtos = _mapper.Map<IEnumerable<DTOCity>>(entities);
      return BaseResponse<IEnumerable<DTOCity>>.SuccessResponse(dtos);
    }

    public async Task<BaseResponse<IEnumerable<DTOCity>>> GetCityByNameAsync(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return BaseResponse<IEnumerable<DTOCity>>.ErrorResponse("Il nome della città non può essere vuoto.", 400); // Aggiunto il codice di stato
      }

      var city = await _cityRepository.GetCityByNameAsync(name);
      if (city == null)
      {
        return BaseResponse<IEnumerable<DTOCity>>.NotFoundResponse($"Nessuna città trovata con il nome '{name}'.", 404); // Aggiunto il codice di stato
      }

      var cityDto = _mapper.Map<DTOCity>(city);
      return BaseResponse<IEnumerable<DTOCity>>.SuccessResponse(new List<DTOCity> { cityDto });
    }
    public override async Task<BaseResponse<DTOCity>> AddAsync(DTOCity entityDto)
    {
      // Verifica se esiste già una città con lo stesso nome
      if (await _cityRepository.CityNameExistsAsync(entityDto.Name))
      {
        return BaseResponse<DTOCity>.ErrorResponse($"Esiste già una città con il nome '{entityDto.Name}'.", 400); // Aggiunto il codice di stato
      }

      // Se non esiste, procedi con l'aggiunta
      return await base.AddAsync(entityDto);
    }
  }
}
