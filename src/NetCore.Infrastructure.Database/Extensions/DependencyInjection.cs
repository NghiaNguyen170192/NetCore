using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Domain.IRepositories;
using NetCore.Domain.SharedKernel;
using NetCore.Infrastructure.Database.AppSettingConfigurations;
using NetCore.Infrastructure.Database.Repositories;
using Redis.OM;

namespace NetCore.Infrastructure.Database.Extensions;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		var databaseConfiguration = new DatabaseConfiguration();
		configuration.GetSection("Database").Bind(databaseConfiguration);

		services.AddDbContext<ApplicationDatabaseContext>(builder =>
		{
			builder.UseNpgsql(
				databaseConfiguration.ApplicationConnectionString,
				optionsBuilder =>
				{
					optionsBuilder.MigrationsAssembly(databaseConfiguration.MigrationsAssembly);
					optionsBuilder.EnableRetryOnFailure();
				});
		});

		services.AddScoped<ICountryRepository, CountryRepository>();
		services.AddScoped<IIdempotencyRepository, IdempotencyRepository>();
		services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDatabaseContext>());

		// Cache configuration
		services.Configure<CacheConfiguration>(configuration.GetSection("CacheConfiguration"));

		// Distributed caching with Redis
		// Attempt to register Redis provider; fallback to in-memory cache if Redis types are not available
		try
		{
			services.AddSingleton(new RedisConnectionProvider(databaseConfiguration.RedisConnectionString));
			services.AddScoped(typeof(ICacheRepository<>), typeof(DistributedCacheRepository<>));
		}
		catch
		{
			// If Redis.OM can't be initialized (e.g., missing DocumentAttribute on entities),
			// use in-memory cache implementation as a safe fallback for local development.
			services.AddScoped(typeof(ICacheRepository<>), typeof(InMemoryCacheRepository<>));
		}

		return services;
	}
}