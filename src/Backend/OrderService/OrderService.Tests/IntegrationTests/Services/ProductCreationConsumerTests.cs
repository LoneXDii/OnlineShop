using AutoFixture;
using Confluent.Kafka;
using Moq;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Abstractions.Payments;
using OrderService.Infrastructure.Models;
using OrderService.Infrastructure.Services.MessageBrocker.Consumers;
using OrderService.Tests.Factories;
using Testcontainers.Kafka;

namespace OrderService.Tests.IntegrationTests.Services;

public class ProductCreationConsumerTests : IAsyncLifetime
{
    private readonly KafkaContainer _kafkaContainer;
    private readonly Mock<IPaymentService> _paymentServiceMock = new();
    private readonly Mock<IProducerService> _producerServiceMock = new();
    private ProductCreationConsumer _consumer;
    private readonly Fixture _fixture = new();

    public ProductCreationConsumerTests()
    {
        _kafkaContainer = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka:latest")
            .WithExposedPort(9092)
            .Build();
    }
    
    public async Task InitializeAsync()
    {
        await _kafkaContainer.StartAsync();

        _consumer = IntegrationTestsEntitiesFactory.CreateTestProductCreationConsumer(_kafkaContainer, 
            _paymentServiceMock, _producerServiceMock);
    }

    public async Task DisposeAsync()
    {
        await _kafkaContainer.StopAsync();
    }

    [Fact]
    public async Task ConsumeMessages_WhenCalled_ShouldRegisterProductInStripeAndProduceIt()
    {
        //Arrange
        var product = _fixture.Create<ConsumedProduct>();
        
        using var producer = IntegrationTestsEntitiesFactory.CreateTestProducer<ConsumedProduct>(_kafkaContainer);

        await producer.ProduceAsync("product-creation", new Message<Null, ConsumedProduct>
        {
            Value = product
        });
        
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));
        
        //Act
        Task.Run(() => _consumer.ConsumeMessagesAsync(cancellationTokenSource.Token));
        
        await Task.Delay(2000);
        
        //Assert
        _paymentServiceMock.Verify(service => service.CreateProductAsync(product.Name, product.Price), Times.Once);
        _producerServiceMock.Verify(producer => producer.ProduceProductPriceIdAsync(product.Id, "PriceId", It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}