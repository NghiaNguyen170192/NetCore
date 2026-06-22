using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetCore.Application.Behaviors;

namespace NetCore.Application.Extensions;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly);
			cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
		});

		return services;
	}
}