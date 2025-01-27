using AutoMapper;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.MessageBrocker;
using ProductsService.Domain.Entities;
using ProductsService.Infrastructure.Data;
using ProductsService.Infrastructure.Models;
using ProductsService.Infrastructure.Repositories;
using ProductsService.Infrastructure.Services.MessageBrocker;
using ProductsService.Infrastructure.Services.MessageBrocker.Consumers;
using ProductsService.Infrastructure.Services.MessageBrocker.Serialization;
using Testcontainers.Kafka;
using Testcontainers.MySql;

namespace ProductsService.Tests.Factories;

public static class IntegrationTestsEntitiesFactory
{
    public static IServiceProvider CreateTestServiceProvider(MySqlContainer commandDbContainer, MySqlContainer queryDbContainer)
    {
        var services = new ServiceCollection();
        
        services.AddDbContext<CommandDbContext>(opt =>
                opt.UseMySql(commandDbContainer.GetConnectionString(),
                    new MySqlServerVersion(new Version(8, 0, 36)),
                    opt => opt.EnableRetryOnFailure()),
            ServiceLifetime.Scoped);
        
        services.AddDbContext<QueryDbContext>(opt =>
                opt.UseMySql(queryDbContainer.GetConnectionString(),
                    new MySqlServerVersion(new Version(8, 0, 36)),
                    opt => opt.EnableRetryOnFailure()),
            ServiceLifetime.Scoped);

        services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>))
            .AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>))
            .AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services.BuildServiceProvider();
    }

    public static IProducerService CreateTestProducerService(KafkaContainer kafkaContainer)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Product, ProductCreationDTO>();
        });
        
        var mapper = config.CreateMapper();
        
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = kafkaContainer.GetBootstrapAddress(),
        };

        var loggerMock = new Mock<ILogger<ProducerService>>();
        
        return new ProducerService(producerConfig, mapper, loggerMock.Object);
    }

    internal static PriceIdConsumer CreatePriceIdConsumer(KafkaContainer kafkaContainer, Mock<IQueryRepository<Product>> productQueryRepositoryMock, 
        Mock<ICommandRepository<Product>> productCommandRepositoryMock)
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        
        unitOfWorkMock.Setup(u => u.ProductQueryRepository).Returns(productQueryRepositoryMock.Object);
        unitOfWorkMock.Setup(u => u.ProductCommandRepository).Returns(productCommandRepositoryMock.Object);
        
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        var serviceScopeMock = new Mock<IServiceScope>();
        var serviceProviderMock = new Mock<IServiceProvider>();

        serviceProviderMock.Setup(x => x.GetService(typeof(IUnitOfWork))).Returns(unitOfWorkMock.Object);
        serviceScopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);
        serviceScopeFactoryMock.Setup(x => x.CreateScope()).Returns(serviceScopeMock.Object);
        
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaContainer.GetBootstrapAddress(),
            GroupId = "test-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        var loggerMock = new Mock<ILogger<PriceIdConsumer>>();
        return new PriceIdConsumer(consumerConfig, serviceScopeFactoryMock.Object, loggerMock.Object);
    }
    
    public static IConsumer<Null, ProductCreationDTO> CreateTestConsumer(KafkaContainer kafkaContainer)
    {
        return new ConsumerBuilder<Null, ProductCreationDTO>(
                new ConsumerConfig
                {
                    BootstrapServers = kafkaContainer.GetBootstrapAddress(),
                    GroupId = "test-consumer-group",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                })
            .SetValueDeserializer(new KafkaDeserializer<ProductCreationDTO>())
            .Build();
    }
    
    internal static IProducer<Null, ConsumedPriceId> CreateTestProducer(KafkaContainer kafkaContainer)
    {
        return new ProducerBuilder<Null, ConsumedPriceId>(
                new ProducerConfig { BootstrapServers = kafkaContainer.GetBootstrapAddress() })
            .SetValueSerializer(new KafkaSerializer<ConsumedPriceId>())
            .Build();
    }
}