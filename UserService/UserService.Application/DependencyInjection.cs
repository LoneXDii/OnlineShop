using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using UserService.Application.Mapping;

namespace UserService.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddAutoMapper(typeof(AppMappingProfile))
			.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly))
			.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
			.AddFluentValidationAutoValidation(cfg =>
			{
				cfg.EnableFormBindingSourceAutomaticValidation = true;
				cfg.EnableBodyBindingSourceAutomaticValidation = true;
			});

		return services;
	}
}
