using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Interfaces.Entity;
using Domain.Interfaces.GenericRepository;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity, new()
    {
        private readonly Noor17Context _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(Noor17Context context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            return filter == null
                ? await _dbSet.ToListAsync()
                : await _dbSet.Where(filter).ToListAsync();
        }

        public async Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }

        public Task UpgradeAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}