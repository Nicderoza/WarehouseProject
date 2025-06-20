using AutoMapper;
using Microsoft.Extensions.Logging;
using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Warehouse.Service.Services
{
  public class CategoryService : GenericService<Categories, DTOCategory>, ICategoryService
  {
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoryService> logger)
        : base(categoryRepository, mapper, logger)
    {
      _categoryRepository = categoryRepository;
      _mapper = mapper;
      _logger = logger;
    }

    public async Task<BaseResponse<IEnumerable<DTOCategory>>> GetCategoriesWithProductsAsync()
    {
      var categories = await _categoryRepository.GetCategoriesWithProductsAsync();
      if (categories == null || !categories.Any())
      {
        return BaseResponse<IEnumerable<DTOCategory>>.NotFoundResponse("Nessuna categoria trovata.", 404); // Aggiunto il codice di stato
      }

      var categoryDtos = _mapper.Map<IEnumerable<DTOCategory>>(categories);
      return BaseResponse<IEnumerable<DTOCategory>>.SuccessResponse(categoryDtos);
    }

    public async Task<BaseResponse<DTOCategory>> GetCategoryByNameAsync(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return BaseResponse<DTOCategory>.ErrorResponse("Il nome della categoria non può essere vuoto.", 400); // Aggiunto il codice di stato
      }

      var category = await _categoryRepository.GetCategoryByNameAsync(name);
      if (category == null)
      {
        return BaseResponse<DTOCategory>.NotFoundResponse("Categoria non trovata.", 404); // Aggiunto il codice di stato
      }

      var categoryDto = _mapper.Map<DTOCategory>(category);
      return BaseResponse<DTOCategory>.SuccessResponse(categoryDto);
    }

    public override async Task<BaseResponse<DTOCategory>> AddAsync(DTOCategory entityDto)
    {
      if (await _categoryRepository.CategoryNameExistsAsync(entityDto.CategoryName))
      {
        return BaseResponse<DTOCategory>.ErrorResponse($"Esiste già una categoria con il nome '{entityDto.CategoryName}'.", 400); // Aggiunto il codice di stato
      }

      return await base.AddAsync(entityDto);
    }

    public override async Task<BaseResponse<IEnumerable<DTOCategory>>> GetAllAsync()
    {
      return await base.GetAllAsync();
    }

    public override async Task<BaseResponse<DTOCategory>> GetByIdAsync(int id)
    {
      return await base.GetByIdAsync(id);
    }
  }
}
