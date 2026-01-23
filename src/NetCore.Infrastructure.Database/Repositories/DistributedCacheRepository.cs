#nullable enable

using Microsoft.Extensions.Options;
using NetCore.Domain.IRepositories;
using NetCore.Infrastructure.Database.AppSettingConfigurations;
using Redis.OM;
using Redis.OM.Searching;

namespace NetCore.Infrastructure.Database.Repositories;

/// <summary>
/// Distributed cache repository implementation using Redis with enterprise patterns.
/// </summary>
/// <typeparam name="T">The type of entity to cache.</typeparam>
public class DistributedCacheRepository<T> : ICacheRepository<T>
	where T : class
{
	private readonly RedisCollection<T> _collection;
	private readonly CacheConfiguration _cacheConfig;
	private readonly string _entityName;
	private readonly TimeSpan _defaultTtl;

	/// <summary>
	/// Initializes a new instance of the <see cref="DistributedCacheRepository{T}"/> class.
	/// </summary>
	/// <param name="provider">The Redis connection provider.</param>
	/// <param name="cacheOptions">The cache configuration options.</param>
	public DistributedCacheRepository(RedisConnectionProvider provider, IOptions<CacheConfiguration> cacheOptions)
	{
		_collection = (RedisCollection<T>)provider.RedisCollection<T>();
		_cacheConfig = cacheOptions.Value;
		_entityName = typeof(T).Name.ToLowerInvariant();
		_defaultTtl = TimeSpan.FromMinutes(_cacheConfig.DefaultTtlMinutes);
	}

	/// <summary>
	/// Generates a standardized cache key using enterprise naming conventions.
	/// Format: {prefix}:{environment}:{entity}:{id}
	/// Example: netcore:dev:country:123
	/// </summary>
	/// <param name="id">The entity identifier.</param>
	/// <returns>The formatted cache key.</returns>
	private string GenerateCacheKey(string id)
	{
		return $"{_cacheConfig.KeyPrefix}:{_cacheConfig.Environment}:{_entityName}:{id}";
	}

	/// <summary>
	/// Adds a single item to the cache with the configured TTL.
	/// </summary>
	/// <param name="item">The item to cache.</param>
	/// <returns>The cache key of the inserted item.</returns>
	public async Task<string> AddAsync(T item)
	{
		var result = await _collection.InsertAsync(item, _defaultTtl);
		await _collection.SaveAsync();
		return result;
	}

	/// <summary>
	/// Adds a single item to the cache with a custom TTL.
	/// </summary>
	/// <param name="item">The item to cache.</param>
	/// <param name="ttl">The time to live for this specific item.</param>
	/// <returns>The cache key of the inserted item.</returns>
	public async Task<string> AddAsync(T item, TimeSpan ttl)
	{
		var result = await _collection.InsertAsync(item, ttl);
		await _collection.SaveAsync();
		return result;
	}

	/// <summary>
	/// Adds multiple items to the cache with the configured TTL.
	/// </summary>
	/// <param name="items">The items to cache.</param>
	/// <returns>The cache keys of the inserted items.</returns>
	public async Task<IEnumerable<string>> AddAsync(IEnumerable<T> items)
	{
		var result = await _collection.InsertAsync(items, _defaultTtl);
		await _collection.SaveAsync();
		return result;
	}

	/// <summary>
	/// Adds multiple items to the cache with a custom TTL.
	/// </summary>
	/// <param name="items">The items to cache.</param>
	/// <param name="ttl">The time to live for these items.</param>
	/// <returns>The cache keys of the inserted items.</returns>
	public async Task<IEnumerable<string>> AddAsync(IEnumerable<T> items, TimeSpan ttl)
	{
		var result = await _collection.InsertAsync(items, ttl);
		await _collection.SaveAsync();
		return result;
	}

	/// <summary>
	/// Removes an item from the cache.
	/// </summary>
	/// <param name="item">The item to remove.</param>
	public async Task DeleteAsync(T item)
	{
		await _collection.DeleteAsync(item);
		await _collection.SaveAsync();
	}

	/// <summary>
	/// Removes an item from the cache by its ID.
	/// </summary>
	/// <param name="id">The ID of the item to remove.</param>
	public async Task DeleteByIdAsync(string id)
	{
		var cacheKey = GenerateCacheKey(id);
		var item = await _collection.FindByIdAsync(cacheKey);
		if (item != null)
		{
			await _collection.DeleteAsync(item);
			await _collection.SaveAsync();
		}
	}

	/// <summary>
	/// Retrieves an item from the cache by its ID.
	/// </summary>
	/// <param name="id">The ID of the item to retrieve.</param>
	/// <returns>The cached item, or null if not found.</returns>
	public async Task<T?> FindByIdAsync(string id)
	{
		var cacheKey = GenerateCacheKey(id);
		return await _collection.FindByIdAsync(cacheKey);
	}

	/// <summary>
	/// Retrieves multiple items from the cache by their IDs.
	/// </summary>
	/// <param name="ids">The IDs of the items to retrieve.</param>
	/// <returns>The cached items.</returns>
	public async Task<IEnumerable<T>> FindByIdsAsync(IEnumerable<string> ids)
	{
		var cacheKeys = ids.Select(GenerateCacheKey);
		var result = await _collection.FindByIdsAsync(cacheKeys);
		return result.Values.Where(v => v != null)!;
	}

	/// <summary>
	/// Updates an existing item in the cache and refreshes its TTL.
	/// </summary>
	/// <param name="item">The item to update.</param>
	public async Task UpdateAsync(T item)
	{
		await _collection.UpdateAsync(item);
		await _collection.SaveAsync();
	}

	/// <summary>
	/// Checks if an item exists in the cache.
	/// </summary>
	/// <param name="id">The ID of the item to check.</param>
	/// <returns>True if the item exists, otherwise false.</returns>
	public async Task<bool> ExistsAsync(string id)
	{
		var cacheKey = GenerateCacheKey(id);
		var item = await _collection.FindByIdAsync(cacheKey);
		return item != null;
	}

	/// <summary>
	/// Invalidates all cached items of this entity type.
	/// </summary>
	public async Task InvalidateAllAsync()
	{
		var allItems = _collection.ToList();
		foreach (var item in allItems)
		{
			await _collection.DeleteAsync(item);
		}

		await _collection.SaveAsync();
	}
}