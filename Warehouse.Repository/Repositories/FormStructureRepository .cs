using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using System.Threading.Tasks;
using Warehouse.Data.Repositories;

namespace Warehouse.Repository.Repositories
{
  public class FormStructureRepository : GenericRepository<FormStructures>, IFormStructureRepository
  {
    private readonly IMapper _mapper;

    public FormStructureRepository(WarehouseContext context, IMapper mapper) : base(context)
    {
      _mapper = mapper;
    }

    public async Task<FormStructures?> GetByFormIdAsync(int formId)
    {
      return await _dbSet.FirstOrDefaultAsync(f => f.FormID == formId);
    }

    public async Task UpdateAsync(FormStructures entity)
    {
      _dbSet.Update(entity);
      await _context.SaveChangesAsync();
    }

  }
}
