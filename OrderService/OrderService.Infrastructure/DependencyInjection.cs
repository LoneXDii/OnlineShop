using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OrderService.Infrastructure.Configuration;
using OrderService.Infrastructure.Services;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Abstractions.Payments;
using Stripe;

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

        services.AddSingleton<IOrderRepository, MongoOrderRepository>()
            .AddSingleton<IProductService, FakeProductService>()
            .AddScoped<IPaymentService, PaymentService>()
            .AddScoped<ITemporaryStorageService, SessionStorageService>();

        services.AddDistributedMemoryCache()
            .AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

        return services;
    }
}