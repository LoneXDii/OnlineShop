using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OrderService.Infrastructure.Configuration;
using OrderService.Domain.Abstractions;
using OrderService.Infrastructure.Services;

namespace OrderService.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<MongoDBSettings>(options => configuration.GetSection("MongoDB").Bind(options));
		services.AddSingleton<IDbService, MongoDBService>();

		return services;
	}
}