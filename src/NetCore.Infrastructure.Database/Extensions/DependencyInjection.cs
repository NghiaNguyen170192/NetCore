using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Domain.IRepositories;
using NetCore.Domain.SharedKernel;
using NetCore.Infrastructure.Database.AppSettingConfigurations;
using NetCore.Infrastructure.Database.Repositories;
using Redis.OM;

namespace NetCore.Infrastructure.Database.Extensions;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, DatabaseConfiguration databaseConfiguration)
	{
		services.AddDbContext<ApplicationDatabaseContext>(builder =>
		{
			builder.UseSqlServer(databaseConfiguration.ApplicationConnectionString,
				optionsBuilder => optionsBuilder.MigrationsAssembly(databaseConfiguration.MigrationsAssembly));
		});

		services.AddScoped<ICountryRepository, CountryRepository>();
		services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDatabaseContext>());

		//Caching
		services.AddSingleton(new RedisConnectionProvider(databaseConfiguration.RedisConnectionString));
		services.AddScoped(typeof(ICacheRepository<>), typeof(DistributedCacheRepository<>));

		return services;
	}
}