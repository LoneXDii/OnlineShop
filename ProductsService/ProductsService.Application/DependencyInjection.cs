using Microsoft.Extensions.DependencyInjection;
using ProductsService.Application.Mapping;

namespace ProductsService.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly))
			.AddAutoMapper(typeof(AppMappingProfile));

		return services;
	}
}
