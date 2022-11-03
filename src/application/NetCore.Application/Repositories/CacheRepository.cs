using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetCore.Application.Repositories.Interfaces;
using NetCore.Infrastructure.Database.Commons;
using StackExchange.Redis;
using System.Linq.Expressions;
using System.Text.Json;

namespace NetCore.Application.Repositories;

public class CacheRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : BaseEntity
{
	private readonly ConnectionMultiplexer _multiplexer;
	private readonly IDatabase _cacheDatabase;

	public IQueryable<TEntity> Collection => throw new NotImplementedException();

	public CacheRepository(ConnectionMultiplexer multiplexer)
	{
		_multiplexer = multiplexer;
		_cacheDatabase = multiplexer.GetDatabase();
	}

	public async Task AddAsync(TEntity entity)
	{
		await _cacheDatabase.StringSetAsync(entity.Id.ToString(), JsonSerializer.Serialize(entity));
	}

	public async Task AddRangeAsync(IEnumerable<TEntity> entities)
	{
		foreach (var entity in entities)
		{
			await _cacheDatabase.StringSetAsync(entity.Id.ToString(), JsonSerializer.Serialize(entity));
		}
	}

	public async Task<TEntity> GetByIdAsync(Guid id)
	{
		var redisValue = await _cacheDatabase.StringGetAsync(id.ToString());

		if (!redisValue.IsNullOrEmpty)
		{
			return JsonSerializer.Deserialize<TEntity>(redisValue.ToString(), new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
		}

		return null;
	}

	public async Task Remove(TEntity entity)
	{
		await _cacheDatabase.KeyDeleteAsync(entity.Id.ToString());
	}

	public async Task RemoveRange(IEnumerable<TEntity> entities)
	{
		foreach (var entity in entities)
		{
			await _cacheDatabase.KeyDeleteAsync(entity.Id.ToString());
		}
	}

	public async Task Update(TEntity entity)
	{
		await _cacheDatabase.StringSetAsync(entity.Id.ToString(), JsonSerializer.Serialize(entity));
	}

	public async Task UpdateRange(IEnumerable<TEntity> entities)
	{
		foreach (var entity in entities)
		{
			await _cacheDatabase.StringSetAsync(entity.Id.ToString(), JsonSerializer.Serialize(entity));
		}
	}
	
	public void Dispose()
	{
		_multiplexer.Dispose();
		GC.SuppressFinalize(this);
	}

	Task<EntityEntry<TEntity>> IRepository<TEntity>.AddAsync(TEntity entity)
	{
		throw new NotImplementedException();
	}

	EntityEntry<TEntity> IRepository<TEntity>.Remove(TEntity entity)
	{
		throw new NotImplementedException();
	}

	void IRepository<TEntity>.RemoveRange(IEnumerable<TEntity> entities)
	{
		throw new NotImplementedException();
	}

	public Task<IReadOnlyList<TEntity>> GetAllAsync()
	{
		throw new NotImplementedException();
	}

	EntityEntry<TEntity> IRepository<TEntity>.Update(TEntity entity)
	{
		throw new NotImplementedException();
	}

	void IRepository<TEntity>.UpdateRange(IEnumerable<TEntity> entities)
	{
		throw new NotImplementedException();
	}

	public int SaveChanges()
	{
		throw new NotImplementedException();
	}

	public Task<int> SaveChangesAsync()
	{
		throw new NotImplementedException();
	}

	public Task<bool> ExistAsync(Expression<Func<TEntity, bool>> expression)
	{
		throw new NotImplementedException();
	}
}
