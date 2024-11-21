using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using ProductsService.Application.Mapping;
using System.Reflection;

namespace ProductsService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly))
            .AddAutoMapper(typeof(AppMappingProfile))
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddFluentValidationAutoValidation(cfg =>
            {
                cfg.EnableFormBindingSourceAutomaticValidation = true;
                cfg.EnableBodyBindingSourceAutomaticValidation = true;
            });

        return services;
    }
}
