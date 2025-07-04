using AutoMapper;
using Microsoft.Extensions.Logging;
using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using System.Threading.Tasks;

namespace Warehouse.Service.Services
{
  public class FormStructureService : GenericService<FormStructures, DTOFormStructure>, IFormStructureService
  {
    private readonly IFormStructureRepository _formRepo;

    public FormStructureService(IFormStructureRepository repository, IMapper mapper, ILogger<FormStructureService> logger)
        : base(repository, mapper, logger)
    {
      _formRepo = repository;
    }

    public async Task<BaseResponse<DTOFormStructure>> GetByFormIdAsync(int FormID)
    {
      var entity = await _formRepo.GetByFormIdAsync(FormID);
      if (entity == null)
        return BaseResponse<DTOFormStructure>.NotFoundResponse("Form non trovato.");

      var dto = new DTOFormStructure
      {
        FormID = entity.FormID,
        ProductForm = entity.ProductForm,
        JsonContent = entity.JsonContent
      };

      return BaseResponse<DTOFormStructure>.SuccessResponse(dto, "Form trovato.");
    }

    public async Task<BaseResponse<DTOFormStructure>> UpdateAsync(int formId, DTOFormStructure dto)
    {
      var existingEntity = await _formRepo.GetByIdAsync(formId);
      if (existingEntity == null)
        return BaseResponse<DTOFormStructure>.NotFoundResponse("Form non trovato.");

      existingEntity.ProductForm = dto.ProductForm;
      existingEntity.JsonContent = dto.JsonContent;

      await _formRepo.UpdateAsync(existingEntity);

      var updatedDto = _mapper.Map<DTOFormStructure>(existingEntity);
      return BaseResponse<DTOFormStructure>.SuccessResponse(updatedDto, "Form aggiornato.");
    }


  }
}
