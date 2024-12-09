using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Infrastructure.Configuration;
using ProductsService.Infrastructure.Data;
using ProductsService.Infrastructure.Repositories;
using ProductsService.Infrastructure.Services;
using ProductsService.Infrastructure.Specifications;

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
            .AddScoped<ISpecificationFactory, SpecificationFactory>()
            .AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<BlobServiceOptions>(options => configuration.GetSection("Blobs").Bind(options));

        return services;
    }
}
