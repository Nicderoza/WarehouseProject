using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;
using System.Threading.Tasks;

namespace Warehouse.Interfaces.IServices
{
  public interface IFormStructureService : IGenericService<DTOFormStructure>
  {
    Task<BaseResponse<DTOFormStructure>> GetByFormIdAsync(int formId);
    Task<BaseResponse<DTOFormStructure>> UpdateAsync(int formId, DTOFormStructure dto);

  }
}
