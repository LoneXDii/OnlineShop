using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Mapping;

namespace UserService.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddAutoMapper(typeof(AppMappingProfile))
			.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));

		return services;
	}
}
