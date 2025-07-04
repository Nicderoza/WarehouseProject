using Warehouse.Data.Models;

namespace Warehouse.Interfaces.IRepositories
{
  public interface IFormStructureRepository : IGenericRepository<FormStructures>
  {
    Task<FormStructures?> GetByFormIdAsync(int formId);
    Task UpdateAsync(FormStructures entity);

  }
}
