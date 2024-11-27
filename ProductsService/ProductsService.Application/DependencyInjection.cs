using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using ProductsService.Application.Mapping;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using ProductsService.Application.Configuration;

namespace ProductsService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly))
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddFluentValidationAutoValidation(cfg =>
            {
                cfg.EnableFormBindingSourceAutomaticValidation = true;
                cfg.EnableBodyBindingSourceAutomaticValidation = true;
            });

        services.Configure<PaginationSettings>(options => configuration.GetSection("Pagination").Bind(options));

        services.AddAutoMapper(
            typeof(ProductMappingProfile),
            typeof(CategoryMappingProfile),
            typeof(AttributeValueMappingProfile),
            typeof(CategoryAttributeValuesMappingProfile));

        return services;
    }
}
