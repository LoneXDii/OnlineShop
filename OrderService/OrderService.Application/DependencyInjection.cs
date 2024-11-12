using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Mapping;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;

namespace OrderService.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddAutoMapper(typeof(AppMappingProfile))
				.AddMediatR(cfg =>
					cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly))
				.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
				.AddFluentValidationAutoValidation(cfg =>
				{
					cfg.EnableFormBindingSourceAutomaticValidation = true;
					cfg.EnableBodyBindingSourceAutomaticValidation = true;
				});

		return services;
	}
}