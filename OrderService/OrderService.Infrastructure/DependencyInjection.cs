using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OrderService.Infrastructure.Configuration;
using OrderService.Infrastructure.Services;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Abstractions.Payments;
using Stripe;
using OrderService.Infrastructure.Repositories;
using OrderService.Infrastructure.Mapping;
using AutoMapper.Extensions.ExpressionMapping;

namespace OrderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDBSettings>(options => configuration.GetSection("MongoDB").Bind(options))
            .Configure<StripeSettings>(options => configuration.GetSection("Stripe").Bind(options))
            .Configure<GrpcSettings>(options => configuration.GetSection("gRPC").Bind(options));

        services.AddScoped<CustomerService>()
            .AddScoped<Stripe.ProductService>()
            .AddScoped<PriceService>();

        services.AddSingleton<IOrderRepository, MongoOrderRepository>()
            .AddSingleton<IProductService, GrpcProductService>()
            .AddScoped<IPaymentService, PaymentService>()
            .AddScoped<ITemporaryStorageService, RedisStorageService>();

        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = configuration["Redis:Configuration"];
            opt.InstanceName = configuration["Redis:InstanceName"];
        });

        services.AddAutoMapper(cfg =>
        {
            cfg.AddExpressionMapping();
        },typeof(MongoOrderMappingProfile));

        return services;
    }
}