using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NetCore.Application.Extensions;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddDispatcher(typeof(AssemblyReference).Assembly);
		return services;
	}
}
