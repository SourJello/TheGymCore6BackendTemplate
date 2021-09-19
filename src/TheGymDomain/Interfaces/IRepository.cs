using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TheGymDomain.Models.Common;

namespace TheGymDomain.Interfaces
{
    public interface IRepository<TEntity> where TEntity: Entity
    {
        TEntity GetById(Guid id);
        Task<TEntity> GetByIdAsync(Guid id);
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        void Add(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task<TEntity> AddAsync(TEntity entity);
        void Remove(TEntity entity);
        void Remove(Guid id); 
        Task RemoveRangeAsync(IEnumerable<Guid> ids);
        void RemoveRange(IEnumerable<TEntity> entities);
        Task RemoveAsync(Guid id);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

    }
}
