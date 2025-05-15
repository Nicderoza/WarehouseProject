using Warehouse.Common.DTOs;
using Warehouse.Common.Responses; // Assicurati che questo using sia presente
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Warehouse.Interfaces.IServices
{
  public interface ICityService : IGenericService<DTOCity>
  {
    Task<BaseResponse<IEnumerable<DTOCity>>> GetCityByNameAsync(string name);
  }
}
