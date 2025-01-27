using System.Security.Claims;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Abstractions.Payments;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Mapping;
using OrderService.Infrastructure.Models;
using OrderService.Infrastructure.Repositories;
using OrderService.Infrastructure.Services;
using OrderService.Infrastructure.Services.MessageBrocker;
using OrderService.Infrastructure.Services.MessageBrocker.Consumers;
using OrderService.Infrastructure.Services.MessageBrocker.Serialization;
using Testcontainers.Kafka;
using Testcontainers.MongoDb;
using Testcontainers.Redis;

namespace OrderService.Tests.Factories;

public static class IntegrationTestsEntitiesFactory
{
    public static IProducerService CreateTestProducerService(KafkaContainer kafkaContainer)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<OrderEntity, ProducedOrderDTO>();
        });
        
        var mapper = config.CreateMapper();
        
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = kafkaContainer.GetBootstrapAddress(),
        };

        var loggerMock = new Mock<ILogger<ProducerService>>();
        
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, "test@example.com")
        };
        
        var claimsIdentity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        
        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };

        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
        
        return new ProducerService(producerConfig, mapper, httpContextAccessorMock.Object, loggerMock.Object);
    }

    public static IServiceProvider CreateServiceProviderForMongoDbTests(MongoDbContainer mongoDbContainer)
    {
        var services = new ServiceCollection();
        
        services.AddSingleton(serviceProvider =>
        {
            var client = new MongoClient(mongoDbContainer.GetConnectionString());
            var database = client.GetDatabase("OrdersDB");

            return database.GetCollection<Order>("orders");
        });
        
        services.AddAutoMapper(cfg =>
        {
            cfg.AddExpressionMapping();
        },typeof(OrderMappingProfile));

        services.AddSingleton<IOrderRepository, OrderRepository>();
        
        return services.BuildServiceProvider();
    }
    
    internal static ProductCreationConsumer CreateTestProductCreationConsumer(KafkaContainer kafkaContainer, 
        Mock<IPaymentService> paymentServiceMock, Mock<IProducerService> producerServiceMock)
    {
        var serviceScopeFactoryMock = CreateServiceScopeFactoryMock(paymentServiceMock, producerServiceMock);
        
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaContainer.GetBootstrapAddress(),
            GroupId = "test-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        var loggerMock = new Mock<ILogger<ProductCreationConsumer>>();
        
        return new ProductCreationConsumer(consumerConfig, serviceScopeFactoryMock.Object, loggerMock.Object);
    }

    internal static UserCreationConsumer CreateTestUserCreationConsumer(KafkaContainer kafkaContainer,
        Mock<IPaymentService> paymentServiceMock, Mock<IProducerService> producerServiceMock)
    {
        var serviceScopeFactoryMock = CreateServiceScopeFactoryMock(paymentServiceMock, producerServiceMock);
        
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaContainer.GetBootstrapAddress(),
            GroupId = "test-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        var loggerMock = new Mock<ILogger<UserCreationConsumer>>();
        
        return new UserCreationConsumer(consumerConfig, serviceScopeFactoryMock.Object, loggerMock.Object);
    }
    
    public static IConsumer<Null, T> CreateTestConsumer<T>(KafkaContainer kafkaContainer)
    {
        return new ConsumerBuilder<Null, T>(
                new ConsumerConfig
                {
                    BootstrapServers = kafkaContainer.GetBootstrapAddress(),
                    GroupId = "test-consumer-group",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                })
            .SetValueDeserializer(new KafkaDeserializer<T>())
            .Build();
    }
    
    public static IProducer<Null, T> CreateTestProducer<T>(KafkaContainer kafkaContainer)
    {
        return new ProducerBuilder<Null, T>(
                new ProducerConfig { BootstrapServers = kafkaContainer.GetBootstrapAddress() })
            .SetValueSerializer(new KafkaSerializer<T>())
            .Build();
    }
    
    public static IServiceProvider CreateServiceProviderForRedisTests(RedisContainer redisContainer, IHttpContextAccessor httpContextAccessor)
    {
        var services = new ServiceCollection();
        
        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = redisContainer.GetConnectionString();
            opt.InstanceName = "cart";
        });
        
        services.AddSingleton(httpContextAccessor);
        
        var loggerMock = new Mock<ILogger<RedisStorageService>>();
        services.AddSingleton(loggerMock.Object);
        
        services.AddScoped<ITemporaryStorageService, RedisStorageService>();
        
        return services.BuildServiceProvider();
    }
    
    private static Mock<IServiceScopeFactory> CreateServiceScopeFactoryMock(Mock<IPaymentService> paymentServiceMock, Mock<IProducerService> producerServiceMock)
    {
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        var serviceScopeMock = new Mock<IServiceScope>();
        
        var services = new ServiceCollection();
        services.AddScoped<IPaymentService>(sp => paymentServiceMock.Object);
        services.AddScoped<IProducerService>(sp => producerServiceMock.Object);
        var serviceProvider = services.BuildServiceProvider();
        
        serviceScopeFactoryMock.Setup(factory => factory.CreateScope()).Returns(serviceScopeMock.Object);
        
        serviceScopeMock.Setup(scope => scope.ServiceProvider).Returns(serviceProvider);
        
        paymentServiceMock.Setup(service => service.CreateProductAsync(It.IsAny<string>(), It.IsAny<double>()))
            .ReturnsAsync("PriceId");
        
        paymentServiceMock.Setup(service => service.CreateCustomerAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("StripeId");
        
        return serviceScopeFactoryMock;
    }
}