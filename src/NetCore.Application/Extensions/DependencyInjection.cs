using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NetCore.Application.Extensions;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		return services;
	}
}
