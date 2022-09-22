using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetCore.Infrastructure.Database.Commons;

namespace NetCore.Infrastructure.Database.Repositories;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    IQueryable<TEntity> Collection { get; }

    Task<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    EntityEntry<TEntity> Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    Task<IReadOnlyList<TEntity>> GetAllAsync();

    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    EntityEntry<TEntity> Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    Task<bool> ExistAsync(Expression<Func<TEntity, bool>> expression);
}
