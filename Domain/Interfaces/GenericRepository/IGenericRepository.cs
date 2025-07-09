using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Interfaces.Entity;

namespace Domain.Interfaces.GenericRepository
{
    public interface IGenericRepository<T> where T:class,IEntity,new()
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>> filter = null);
        Task<T> GetByFilterAsync(Expression<Func<T,bool>> filter);
        Task AddAsync(T entity);
        Task DeleteAsync(string id);
        Task UpgradeAsync(string id);

    }
}