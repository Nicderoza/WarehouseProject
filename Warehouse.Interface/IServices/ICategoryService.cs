using Warehouse.Common.DTOs;
using Warehouse.Common.Responses;

namespace Warehouse.Interfaces.IServices
{
  public interface ICategoryService : IGenericService<DTOCategory>
  {
    Task<BaseResponse<DTOCategory>> GetCategoryByNameAsync(string name);

    Task<BaseResponse<IEnumerable<DTOCategory>>> GetCategoriesWithProductsAsync();

  }
}
