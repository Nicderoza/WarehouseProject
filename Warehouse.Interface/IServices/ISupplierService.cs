
﻿using Warehouse.Interfaces.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Common.Responses;
using Warehouse.Common.DTOs;
using Warehouse.Data.Models;
﻿using Warehouse.Common.DTOs;
using Warehouse.Data.Models;

namespace Warehouse.Interfaces.IServices
{
    public interface ISupplierService : IGenericService<DTOSupplier>
    {
        Task<BaseResponse<IEnumerable<DTOSupplier>>> GetSuppliersByCityAsync(int cityId);
    }
}
