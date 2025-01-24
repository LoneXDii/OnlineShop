using AutoMapper;
using Confluent.Kafka;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Testcontainers.Kafka;
using Testcontainers.Redis;
using UserService.DAL.Entities;
using UserService.DAL.Mapping;
using UserService.DAL.Models;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.MessageBrocker.Consumers;
using UserService.DAL.Services.MessageBrocker.ProducerService;
using UserService.DAL.Services.MessageBrocker.Serialization;
using UserService.DAL.Services.TemporaryStorage;

namespace UserService.Tests.Factories;

public static class IntegrationTestsEntitiesFactory
{
    public static IServiceProvider CreateServiceProviderForRedisTests(RedisContainer redisContainer)
    {
        var services = new ServiceCollection();
        
        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = redisContainer.GetConnectionString();
            opt.InstanceName = "codes";
        });
        
        var loggerMock = new Mock<ILogger<CacheService>>();
        services.AddSingleton(loggerMock.Object);
        
        services.AddScoped<ICacheService, CacheService>();
        
        return services.BuildServiceProvider();
    }
    
    public static IProducerService CreateTestProducerService(KafkaContainer kafkaContainer)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MqUserRequestMappingProfile());
        });
        
        var mapper = config.CreateMapper();
        
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = kafkaContainer.GetBootstrapAddress(),
        };

        var loggerMock = new Mock<ILogger<ProducerService>>();
        
        return new ProducerService(producerConfig, mapper, loggerMock.Object);
    }
    
    internal static OrderActionConsumer CreateTestOrderActionsConsumer(KafkaContainer kafkaContainer, Mock<IEmailService> emailServiceMock)
    {
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        
        var serviceScopeMock = new Mock<IServiceScope>();
        
        var services = new ServiceCollection();
        services.AddScoped<IEmailService>(sp => emailServiceMock.Object);
        
        var serviceProvider = services.BuildServiceProvider();
        
        serviceScopeFactoryMock.Setup(factory => factory.CreateScope()).Returns(serviceScopeMock.Object);
        
        serviceScopeMock.Setup(scope => scope.ServiceProvider).Returns(serviceProvider);
        
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaContainer.GetBootstrapAddress(),
            GroupId = "test-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        var loggerMock = new Mock<ILogger<OrderActionConsumer>>();
        
        return new OrderActionConsumer(consumerConfig, serviceScopeFactoryMock.Object, loggerMock.Object);
    }
    
    internal static StripeIdConsumer CreateTestStripeIdConsumer(KafkaContainer kafkaContainer, Mock<UserManager<AppUser>> userManagerMock)
    {
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        
        var serviceScopeMock = new Mock<IServiceScope>();
        
        var services = new ServiceCollection();
        services.AddScoped<UserManager<AppUser>>(sp => userManagerMock.Object);
        
        var serviceProvider = services.BuildServiceProvider();
        
        serviceScopeFactoryMock.Setup(factory => factory.CreateScope()).Returns(serviceScopeMock.Object);
        
        serviceScopeMock.Setup(scope => scope.ServiceProvider).Returns(serviceProvider);
        
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaContainer.GetBootstrapAddress(),
            GroupId = "test-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        var loggerMock = new Mock<ILogger<StripeIdConsumer>>();
        
        return new StripeIdConsumer(consumerConfig, serviceScopeFactoryMock.Object, loggerMock.Object);
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
}