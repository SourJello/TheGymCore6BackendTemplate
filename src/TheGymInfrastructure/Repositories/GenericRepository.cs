using TheGymDomain.Models.Common;
using TheGymDomain.Interfaces;
using System.Linq.Expressions;
using TheGymInfrastructure.Persistence.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace TheGymInfrastructure.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected TheGymContext _context;
        protected DbSet<TEntity> _dbset;

        public GenericRepository(TheGymContext context)
        {
            _context = context;
            _dbset = context.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            _dbset.Add(entity);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var ent = await _dbset.AddAsync(entity);
            return ent.Entity;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbset.AddRangeAsync(entities);
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbset;
            if (filter != null)
            {
                query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.AsNoTrackingWithIdentityResolution().ToList();
            }
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbset;
            if (filter != null)
            {
                query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.AsNoTrackingWithIdentityResolution().ToListAsync();
            }
        }

        public TEntity GetById(Guid id)
        {
            return _dbset.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbset.FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            if(_context.Entry(entity).State == EntityState.Detached)
            {
                _dbset.Attach(entity);
            }
            _dbset.Remove(entity);
        }

        public void Remove(Guid id)
        {
            Remove(GetById(id));
        }

        public async Task RemoveAsync(Guid id)
        {
            Remove(await GetByIdAsync(id));
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach(var entity in entities)
            {
                if(_context.Entry(entity).State == EntityState.Detached)
                {
                    _dbset.Attach(entity);
                }
            }

            _dbset.RemoveRange(entities);
        }

        public async Task RemoveRangeAsync(IEnumerable<Guid> ids)
        {
            var entities = new List<TEntity>();
            foreach (var id in ids)
            {
                entities.Add(await GetByIdAsync(id));
            }
            _dbset.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            var trackedEntity = _dbset.Find(entity.Id);
            _context.Entry(trackedEntity).CurrentValues.SetValues(entity);
        }
        public async void UpdateAsync(TEntity entity)
        {
            var trackedEntity = await _dbset.FindAsync(entity.Id);
            _context.Entry(trackedEntity).CurrentValues.SetValues(entity);
        }
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach(var entity in entities)
            {
                var trackedEntity = _dbset.Find(entity.Id);
                _context.Entry(trackedEntity).CurrentValues.SetValues(entity);
            }    
        }
        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                var trackedEntity = await _dbset.FindAsync(entity.Id);
                _context.Entry(trackedEntity).CurrentValues.SetValues(entity);
            }
        }
    }
}
