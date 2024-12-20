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
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Stripe.Tax;
using OrderService.Infrastructure.Models;

namespace OrderService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDBSettings>(options => configuration.GetSection("MongoDB").Bind(options))
            .Configure<StripeSettings>(options => configuration.GetSection("Stripe").Bind(options));

        services.AddScoped<CustomerService>()
            .AddScoped<Stripe.ProductService>()
            .AddScoped<PriceService>();

        services.AddSingleton<IOrderRepository, OrderRepository>()
            .AddSingleton<IProductService, FakeProductService>()
            .AddScoped<IPaymentService, PaymentService>()
            .AddScoped<ITemporaryStorageService, SessionStorageService>();

        services.AddSingleton(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>().Value;
            var client = new MongoClient(settings.ConnectionURI);
            var database = client.GetDatabase(settings.DatabaseName);

            return database.GetCollection<Order>(settings.CollectionName);
        });

        services.AddDistributedMemoryCache()
            .AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

        services.AddAutoMapper(cfg =>
        {
            cfg.AddExpressionMapping();
        },typeof(OrderMappingProfile));

        return services;
    }
}