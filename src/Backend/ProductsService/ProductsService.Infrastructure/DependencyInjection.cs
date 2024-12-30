using Azure.Storage.Blobs;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.MessageBrocker;
using ProductsService.Infrastructure.Configuration;
using ProductsService.Infrastructure.Data;
using ProductsService.Infrastructure.Mapping;
using ProductsService.Infrastructure.Repositories;
using ProductsService.Infrastructure.Services;
using ProductsService.Infrastructure.Services.MessageBrocker;
using ProductsService.Infrastructure.Services.MessageBrocker.Consumers;

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

        services.AddAutoMapper(typeof(ProductCreationDtoMappingProfile));

        services.AddScoped<IBlobService, BlobService>()
            .AddScoped(_ => new BlobServiceClient(configuration["ConnectionStrings:AzureConnection"]));

        services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>))
            .AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>))
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IProducerService, ProducerService>();

        services.Configure<BlobServiceOptions>(options => configuration.GetSection("Blobs").Bind(options));

        services.AddSingleton(serviceProvider =>
        {
            return new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:Server"],
                AllowAutoCreateTopics = true,
                Acks = Acks.All
            };
        });

        services.AddSingleton(serviceProvider =>
        {
            return new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:Server"],
                GroupId = "products-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        });

        services.AddHostedService<PriceIdConsumer>();

        return services;
    }
}
