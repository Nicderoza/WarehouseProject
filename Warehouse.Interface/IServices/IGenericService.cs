using Warehouse.Common.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Common.DTOs;

namespace Warehouse.Interfaces.IServices
{
    public interface IGenericService<TDTO> where TDTO : class
    {
        Task<BaseResponse<IEnumerable<TDTO>>> GetAllAsync();
        Task<BaseResponse<TDTO>> GetByIdAsync(int id);
    
        Task<BaseResponse<TDTO>> AddAsync(TDTO dto);
        Task<BaseResponse<TDTO>> UpdateAsync(int id, TDTO dto);
        Task<BaseResponse<bool>> DeleteAsync(int id);
    
    }
}
