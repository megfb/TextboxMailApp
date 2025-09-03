using Microsoft.EntityFrameworkCore;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Domain.Entities.Common;

namespace TextboxMailApp.Persistence
{
    public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class, IEntity, new()
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<T> CreateAsync(T entity)
        {
            var entry = await _dbSet.AddAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllByPageAsync(int pageNumber, int pageSize)
        {
            return await _dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id)!;
        }

        public async Task SaveRangeAsync(IEnumerable<T> entity)
        {
            await _dbSet.AddRangeAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
