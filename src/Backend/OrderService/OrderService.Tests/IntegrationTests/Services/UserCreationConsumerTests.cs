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

public class UserCreationConsumerTests : IAsyncLifetime
{
    private readonly KafkaContainer _kafkaContainer;
    private readonly Mock<IPaymentService> _paymentServiceMock = new();
    private readonly Mock<IProducerService> _producerServiceMock = new();
    private UserCreationConsumer _consumer;
    private readonly Fixture _fixture = new();

    public UserCreationConsumerTests()
    {
        _kafkaContainer = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka:latest")
            .WithExposedPort(9092)
            .Build();
    }
    
    public async Task InitializeAsync()
    {
        await _kafkaContainer.StartAsync();

        _consumer = IntegrationTestsEntitiesFactory.CreateTestUserCreationConsumer(_kafkaContainer, 
            _paymentServiceMock, _producerServiceMock);
    }

    public async Task DisposeAsync()
    {
        await _kafkaContainer.StopAsync();
    }

    [Fact]
    public async Task ConsumeMessages_WhenCalled_ShouldRegisterUserInStripeAndProduceIt()
    {
        //Arrange
        var user = _fixture.Create<ConsumedUser>();
        
        using var producer = IntegrationTestsEntitiesFactory.CreateTestProducer<ConsumedUser>(_kafkaContainer);

        await producer.ProduceAsync("user-creation", new Message<Null, ConsumedUser>
        {
            Value = user
        });
        
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));
        
        //Act
        Task.Run(() => _consumer.ConsumeMessagesAsync(cancellationTokenSource.Token));
        
        await Task.Delay(2000);
        
        //Assert
        _paymentServiceMock.Verify(service => service.CreateCustomerAsync(user.Email, user.Name), Times.Once);
        _producerServiceMock.Verify(producer => producer.ProduceUserStripeIdAsync(user.Id, "StripeId", It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}