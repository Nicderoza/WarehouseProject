// Warehouse.Interfaces.IRepositories/IGenericRepository.cs
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Warehouse.Interfaces.IRepositories
{
  public interface IGenericRepository<T> where T : class
  {
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id); // Reso nullable per coerenza
    Task<T> AddAsync(T entity); // <<< MODIFICATO: ora restituisce l'entità aggiunta
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<int> SaveChangesAsync(); // <<< MODIFICATO: ora restituisce il numero di modifiche salvate
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
  }
}
