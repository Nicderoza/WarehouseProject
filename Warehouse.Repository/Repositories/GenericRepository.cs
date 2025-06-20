// Warehouse.Data.Repositories/GenericRepository.cs
using Microsoft.EntityFrameworkCore;
using Warehouse.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warehouse.Data; // Necessario per WarehouseContext

namespace Warehouse.Data.Repositories
{
  public class GenericRepository<T> : IGenericRepository<T> where T : class
  {
    protected readonly WarehouseContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(WarehouseContext context)
    {
      _context = context;
      _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
      return await _dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
      return await _dbSet.FindAsync(id);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
      var entry = await _dbSet.AddAsync(entity);
      return entry.Entity; 
    }

    public virtual Task UpdateAsync(T entity)
    {
      _dbSet.Update(entity);
      return Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(int id)
    {
      var entity = await _dbSet.FindAsync(id);
      if (entity != null)
      {
        _dbSet.Remove(entity);
      }
    }

    public async Task<int> SaveChangesAsync()
    {
      return await _context.SaveChangesAsync();
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
      return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate)
    {
      return await _dbSet.Where(predicate).ToListAsync();
    }
  }
}
