using AutoFixture;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Models;
using OrderService.Tests.Factories;
using Testcontainers.Kafka;

namespace OrderService.Tests.IntegrationTests.Services;

public class ProducerServiceTests : IAsyncLifetime
{
    private readonly KafkaContainer _kafkaContainer;
    private readonly Fixture _fixture = new();
    private IProducerService _producerService;
    
    public ProducerServiceTests()
    {
        _kafkaContainer = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka")
            .WithExposedPort(9092)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _kafkaContainer.StartAsync();

        _producerService = IntegrationTestsEntitiesFactory.CreateTestProducerService(_kafkaContainer);
    }

    public async Task DisposeAsync()
    {
        await _kafkaContainer.StopAsync();
    }

    [Fact]
    public async Task ProduceUserStripeIdAsync_WhenCalled_ShouldProduceMessageToKafka()
    {
        //Arrange
        var userId = _fixture.Create<string>();
        var stripeId = _fixture.Create<string>();
        
        //Act
        await _producerService.ProduceUserStripeIdAsync(userId, stripeId);
        
        //Assert
        using var consumer = IntegrationTestsEntitiesFactory.CreateTestConsumer<UserStripeIdDTO>(_kafkaContainer);
        
        consumer.Subscribe("user-stripe-id-creation");
        var result = consumer.Consume(TimeSpan.FromSeconds(10));
        
        Assert.NotNull(result);
        Assert.Equal(userId, result.Message.Value.UserId);
        Assert.Equal(stripeId, result.Message.Value.StripeId);
    }
    
    [Fact]
    public async Task ProduceProductPriceIdAsync_WhenCalled_ShouldProduceMessageToKafka()
    {
        //Arrange
        var productId = _fixture.Create<int>();
        var priceId = _fixture.Create<string>();
        
        //Act
        await _producerService.ProduceProductPriceIdAsync(productId, priceId);
        
        //Assert
        using var consumer = IntegrationTestsEntitiesFactory.CreateTestConsumer<ProductPriceIdDTO>(_kafkaContainer);
        
        consumer.Subscribe("product-price-id-creation");
        var result = consumer.Consume(TimeSpan.FromSeconds(10));
        
        Assert.NotNull(result);
        Assert.Equal(productId, result.Message.Value.Id);
        Assert.Equal(priceId, result.Message.Value.PriceId);
    }

    [Fact]
    public async Task ProduceOrderActionsAsync_WhenCalled_ShouldProduceMessageToKafka()
    {
        //Arrange
        var order = _fixture.Create<OrderEntity>();
        
        //Act
        await _producerService.ProduceOrderActionsAsync(order);
        
        //Assert
        using var consumer = IntegrationTestsEntitiesFactory.CreateTestConsumer<ProducedOrderDTO>(_kafkaContainer);
        
        consumer.Subscribe("order-actions");
        var result = consumer.Consume(TimeSpan.FromSeconds(10));
        
        Assert.NotNull(result);
        Assert.Equal(order.Id, result.Message.Value.Id);
        Assert.Equal(order.OrderStatus, result.Message.Value.OrderStatus);
        Assert.Equal(order.UserId, result.Message.Value.UserId);
        Assert.Equal(order.TotalPrice, result.Message.Value.TotalPrice);
        Assert.Equal("test@example.com", result.Message.Value.UserEmail);
    }
}