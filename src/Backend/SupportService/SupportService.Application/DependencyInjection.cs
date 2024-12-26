using FluentValidation;
using Hangfire;
using Hangfire.MySql;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportService.Application.RequestsPipleneBehavior;
using System.Reflection;

namespace SupportService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly))
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddHangfire(opt => opt.UseStorage(new MySqlStorage(configuration["ConnectionStrings:HangfireDbConnection"], new MySqlStorageOptions())));
        services.AddHangfireServer();

        return services;
    }
}
