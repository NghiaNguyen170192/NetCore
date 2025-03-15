using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NetCore.Application.Extensions;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		var applicationAssembly = typeof(AssemblyReference).GetTypeInfo().Assembly;
		services.AddMediatR(serviceConfiguration =>
		{
			serviceConfiguration.RegisterServicesFromAssembly(applicationAssembly);
		});

		return services;
	}
}
