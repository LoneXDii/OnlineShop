using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SupportService.Domain.Abstractions;
using SupportService.Infrastructure.Data;
using SupportService.Infrastructure.Repositories;
using Testcontainers.MySql;

namespace SupportService.Tests.Factories;

public static class IntegrationTestsEntitiesFactory
{
    public static IServiceProvider CreateTestServiceProvider(MySqlContainer dbContainer)
    {
        var services = new ServiceCollection();
        
        services.AddDbContext<AppDbContext>(opt =>
                opt.UseMySql(dbContainer.GetConnectionString(),
                    new MySqlServerVersion(new Version(8, 0, 36)),
                    opt => opt.EnableRetryOnFailure()),
            ServiceLifetime.Scoped);
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services.BuildServiceProvider();
    }
}