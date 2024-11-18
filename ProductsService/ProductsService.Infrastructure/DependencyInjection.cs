using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Infrastructure.Data;
using ProductsService.Infrastructure.Repositories;
using ProductsService.Infrastructure.Services;


namespace ProductsService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CommandDbContext>(opt =>
                        opt.UseMySql(configuration["ConnectionStrings:MySQLCommandConnection"],
                        new MySqlServerVersion(new Version(8, 0, 36)),
                    opt => opt.EnableRetryOnFailure()),
                    ServiceLifetime.Scoped);

        services.AddDbContext<QueryDbContext>(opt =>
                        opt.UseMySql(configuration["ConnectionStrings:MySQLQueryConnection"],
                        new MySqlServerVersion(new Version(8, 0, 36)),
                    opt => opt.EnableRetryOnFailure()),
                    ServiceLifetime.Scoped);

        services.AddScoped<IBlobService, BlobService>()
            .AddScoped(_ => new BlobServiceClient(configuration["ConnectionStrings:AzureConnection"]));

        services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>))
            .AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>))
            .AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
