using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Mapping;
using OrderService.Application.Sessions;
using OrderService.Domain.Abstractions.Cart;
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

		services.AddDistributedMemoryCache()
			.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(30);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

		services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));

		return services;
	}
}