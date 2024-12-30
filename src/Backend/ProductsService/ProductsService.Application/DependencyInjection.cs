using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using ProductsService.Application.Configuration;
using Hangfire;
using Hangfire.MySql;

namespace ProductsService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly))
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddFluentValidationAutoValidation(cfg =>
            {
                cfg.EnableFormBindingSourceAutomaticValidation = true;
                cfg.EnableBodyBindingSourceAutomaticValidation = true;
            });

        services.Configure<PaginationSettings>(options => configuration.GetSection("Pagination").Bind(options));

        services.AddHangfire(opt => opt.UseStorage(new MySqlStorage(configuration["ConnectionStrings:HangfireDbConnection"], new MySqlStorageOptions())));
        services.AddHangfireServer();

        return services;
    }
}
