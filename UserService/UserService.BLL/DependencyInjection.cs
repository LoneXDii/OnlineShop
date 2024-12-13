using FluentValidation;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using UserService.BLL.Mapping;
using Hangfire.MySql;
using Microsoft.Extensions.Configuration;

namespace UserService.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(RegisterDTOToAppUserMappingProfile), typeof(AppUserToUserInfoDTOMappingProfile), typeof(UpdateUserDTOToAppUserMappingProfile))
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly))
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddFluentValidationAutoValidation(cfg =>
            {
                cfg.EnableFormBindingSourceAutomaticValidation = true;
                cfg.EnableBodyBindingSourceAutomaticValidation = true;
            });

        services.AddHangfire(opt => opt.UseStorage(new MySqlStorage(configuration["ConnectionStrings:HangfireDbConnection"], new MySqlStorageOptions())));
        services.AddHangfireServer();

        return services;
    }
}
