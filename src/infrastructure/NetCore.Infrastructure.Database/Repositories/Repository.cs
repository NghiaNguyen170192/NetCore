using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace NetCore.Infrastructure.Database.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : BaseEntity
    {
        private readonly DatabaseContext _databaseContext;

        public IQueryable<TEntity> Collection => _databaseContext.Set<TEntity>().AsQueryable();

        public Repository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return await _databaseContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            await _databaseContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        public EntityEntry<TEntity> Remove(TEntity entity)
        {
            return _databaseContext.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _databaseContext.Set<TEntity>().RemoveRange(entities);
        }

        public Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _databaseContext.Set<TEntity>().FindAsync(id, cancellationToken);
        }

        public EntityEntry<TEntity> Update(TEntity entity)
        {
            return _databaseContext.Set<TEntity>().Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _databaseContext.Set<TEntity>().UpdateRange(entities);
        }

        public int SaveChanges()
        {
            return _databaseContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _databaseContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _databaseContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
