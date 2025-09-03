using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Domain.Entities;
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

        public async Task<T> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id)!;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
