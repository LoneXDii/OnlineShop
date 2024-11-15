using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using UserService.BLL.Mapping;

namespace UserService.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(RegisterDTOToAppUserMappingProfile), typeof(AppUserToUserInfoDTOMappingProfile), typeof(UpdateUserDTOToAppUserMappingProfile))
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
