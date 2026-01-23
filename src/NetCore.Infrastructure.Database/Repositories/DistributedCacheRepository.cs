#nullable enable

using NetCore.Domain.IRepositories;
using Redis.OM.Searching;
using Redis.OM;

namespace NetCore.Infrastructure.Database.Repositories;

public class DistributedCacheRepository<T>(RedisConnectionProvider provider) : ICacheRepository<T>
    where T : class
{
	private readonly RedisCollection<T> _collection = (RedisCollection<T>)provider.RedisCollection<T>();
	private readonly TimeSpan _timeout = TimeSpan.FromHours(1);

    public async Task<string> AddAsync(T item)
	{
		var result = await _collection.InsertAsync(item, _timeout);
		await _collection.SaveAsync();
		return result;
	}

	public async Task<IEnumerable<string>> AddAsync(IEnumerable<T> items)
	{
		var result = await _collection.InsertAsync(items, _timeout);
		await _collection.SaveAsync();
		return result;
	}

	public async Task DeleteAsync(T item)
	{
		await _collection.DeleteAsync(item);
		await _collection.SaveAsync();
	}

	public async Task<T?> FindByIdAsync(string id)
	{
		return await _collection.FindByIdAsync(id);
	}

	//public async Task<IEnumerable<T>> FindByIdsAsync(IEnumerable<string> ids)
	//{
	//	return await _collection.FindByIdsAsync(ids);
	//}

	public async Task UpdateAsync(T item)
	{
		await _collection.UpdateAsync(item);
		await _collection.SaveAsync();
	}
}