using System.Linq.Expressions;
using System.Text.Json;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetCore.Infrastructure.Database;
using NetCore.Infrastructure.Database.Commons;
using StackExchange.Redis;

namespace NetCore.Application.Repositories;

public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : BaseEntity
{
	private readonly DatabaseContext _databaseContext;
	private readonly ConnectionMultiplexer _multiplexer;
	private readonly StackExchange.Redis.IDatabase _redisDatabase;
	private bool _disposed = false;

	public IQueryable<TEntity> Collection => _databaseContext.Set<TEntity>();

	public Repository(DatabaseContext databaseContext, ConnectionMultiplexer multiplexer)
	{
		_databaseContext = databaseContext;
		_multiplexer = multiplexer;
		_redisDatabase = _multiplexer.GetDatabase();
	}

	public async Task<EntityEntry<TEntity>> AddAsync(TEntity entity)
	{
		var result = await _databaseContext.Set<TEntity>().AddAsync(entity);
		await _redisDatabase.StringSetAsync(entity.Id.ToString(), JsonSerializer.Serialize(entity));
		return result;
	}

	public async Task AddRangeAsync(IEnumerable<TEntity> entities)
	{
		await _databaseContext.Set<TEntity>().AddRangeAsync(entities);
		foreach (var entity in entities)
		{
			await _redisDatabase.StringSetAsync(entity.Id.ToString(), JsonSerializer.Serialize(entity));
		}
	}

	public EntityEntry<TEntity> Remove(TEntity entity)
	{
		_redisDatabase.KeyDelete(entity.Id.ToString());
		return _databaseContext.Set<TEntity>().Remove(entity);
	}

	public void RemoveRange(IEnumerable<TEntity> entities)
	{
		_databaseContext.Set<TEntity>().RemoveRange(entities);
		foreach (var entity in entities)
		{
			_redisDatabase.KeyDelete(entity.Id.ToString());
		}
	}

	public Task<IReadOnlyList<TEntity>> GetAllAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<TEntity> GetByIdAsync(Guid id)
	{
		var redisValue = await _redisDatabase.StringGetAsync(id.ToString());
		if (!redisValue.IsNullOrEmpty)
		{
			return JsonSerializer.Deserialize<TEntity>(redisValue, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
		}

		return await _databaseContext.Set<TEntity>().FindAsync(id);
	}

	public EntityEntry<TEntity> Update(TEntity entity)
	{
		var result = _databaseContext.Set<TEntity>().Update(entity);
		_redisDatabase.StringSetAsync(entity.Id.ToString(), JsonSerializer.Serialize(entity));

		return result;
	}

	public void UpdateRange(IEnumerable<TEntity> entities)
	{
		_databaseContext.Set<TEntity>().UpdateRange(entities);
	}

	public int SaveChanges()
	{
		return _databaseContext.SaveChanges();
	}

	public async Task<int> SaveChangesAsync()
	{
		return await _databaseContext.SaveChangesAsync();
	}

	public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> expression)
	{
		return await _databaseContext.Set<TEntity>().AnyAsync(expression);
	}



	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				_databaseContext.Dispose();
				_multiplexer.Dispose();
			}
		}

		_disposed = true;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
