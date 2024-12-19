using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using UserService.DAL.Entities;
using UserService.DAL.Database;
using UserService.DAL.Services.Authentication;
using UserService.DAL.Services.BlobStorage;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;
using UserService.DAL.Models;
using Confluent.Kafka;
using UserService.DAL.Services.MessageBrocker.ProducerService;
using UserService.DAL.Mapping;
using UserService.DAL.Services.MessageBrocker.Consumers;

namespace UserService.DAL;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
                        opt.UseMySql(configuration["ConnectionStrings:MySQLConnection"],
                        new MySqlServerVersion(new Version(8, 0, 36)),
                    opt => opt.EnableRetryOnFailure()),
                    ServiceLifetime.Scoped);

        services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddAutoMapper(typeof(MqUserRequestMappingProfile));

        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = configuration["Redis:Configuration"];
            opt.InstanceName = configuration["Redis:InstanceName"];
        });

        services.AddScoped<IBlobService, BlobService>()
            .AddScoped(_ => new BlobServiceClient(configuration["ConnectionStrings:AzureConnection"]));

        services.AddScoped<ITokenService, TokenService>()
            .AddScoped<ISendGridClient>(sp => new SendGridClient(configuration["EmailAccount:ApiKey"]))
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<ICacheService, CacheService>()
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

        services.AddHostedService<StripeIdConsumer>();

        return services;
    }
}