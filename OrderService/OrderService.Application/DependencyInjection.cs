using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Mapping;
using OrderService.Application.Settings;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;

namespace OrderService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(OrderMappingProfile), typeof(CartMappingProfile))
                .AddMediatR(cfg =>
                    cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly))
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddFluentValidationAutoValidation(cfg =>
                {
                    cfg.EnableFormBindingSourceAutomaticValidation = true;
                    cfg.EnableBodyBindingSourceAutomaticValidation = true;
                });

        services.Configure<PaginationSettings>(options => configuration.GetSection("Pagination").Bind(options));

        return services;
    }
}