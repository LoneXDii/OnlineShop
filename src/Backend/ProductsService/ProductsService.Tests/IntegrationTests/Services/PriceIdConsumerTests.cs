using AutoFixture;
using Confluent.Kafka;
using Moq;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using ProductsService.Infrastructure.Models;
using ProductsService.Infrastructure.Services.MessageBrocker.Consumers;
using ProductsService.Tests.Factories;
using Testcontainers.Kafka;

namespace ProductsService.Tests.IntegrationTests.Services;

public class PriceIdConsumerTests : IAsyncLifetime
{
    private readonly KafkaContainer _kafkaContainer;
    private readonly Mock<IQueryRepository<Product>> _productQueryRepositoryMock = new();
    private readonly Mock<ICommandRepository<Product>> _productCommandRepositoryMock = new();
    private PriceIdConsumer _consumer;
    private readonly Fixture _fixture = new();

    public PriceIdConsumerTests()
    {
        _kafkaContainer = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka:latest")
            .WithExposedPort(9092)
            .Build();
    }
    
    public async Task InitializeAsync()
    {
        await _kafkaContainer.StartAsync();

        _consumer = IntegrationTestsEntitiesFactory.CreatePriceIdConsumer(_kafkaContainer, 
            _productQueryRepositoryMock, _productCommandRepositoryMock);
    }

    public async Task DisposeAsync()
    {
        await _kafkaContainer.StopAsync();
    }
    
    [Fact]
    public async Task ConsumeMessages_WhenProductExists_ShouldUpdatePriceId()
    {
        //Arrange
        var consumedPriceId = _fixture.Create<ConsumedPriceId>();
        var product = new Product { Id = consumedPriceId.Id };
        
        _productQueryRepositoryMock
            .Setup(x => x.GetByIdAsync(consumedPriceId.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        
        using var producer = IntegrationTestsEntitiesFactory.CreateTestProducer(_kafkaContainer);

        await producer.ProduceAsync("product-price-id-creation", new Message<Null, ConsumedPriceId>
        {
            Value = consumedPriceId
        });
        
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));

        Task.Run(() => _consumer.ConsumeMessages(cancellationTokenSource.Token));
        
        await Task.Delay(2000);

        //Assert
        _productQueryRepositoryMock.Verify(x => x.GetByIdAsync(consumedPriceId.Id, It.IsAny<CancellationToken>()), Times.Once);
        _productCommandRepositoryMock.Verify(x => x.UpdateAsync(product, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task ConsumeMessages_WhenProductIsNotExists_ShouldNotUpdatePriceId()
    {
        //Arrange
        var consumedPriceId = _fixture.Create<ConsumedPriceId>();
        
        _productQueryRepositoryMock
            .Setup(x => x.GetByIdAsync(consumedPriceId.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);
        
        using var producer = IntegrationTestsEntitiesFactory.CreateTestProducer(_kafkaContainer);

        await producer.ProduceAsync("product-price-id-creation", new Message<Null, ConsumedPriceId>
        {
            Value = consumedPriceId
        });
        
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));

        Task.Run(() => _consumer.ConsumeMessages(cancellationTokenSource.Token));
        
        await Task.Delay(2000);

        //Assert
        _productQueryRepositoryMock.Verify(x => x.GetByIdAsync(consumedPriceId.Id, It.IsAny<CancellationToken>()), Times.Once);
        _productCommandRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), 
            Times.Never);
    }
}