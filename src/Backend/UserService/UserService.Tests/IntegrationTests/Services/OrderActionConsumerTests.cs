using AutoFixture;
using Confluent.Kafka;
using Moq;
using Testcontainers.Kafka;
using UserService.DAL.Models;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.MessageBrocker.Consumers;
using UserService.Tests.Factories;

namespace UserService.Tests.IntegrationTests.Services;

public class OrderActionConsumerTests : IAsyncLifetime
{
    private readonly KafkaContainer _kafkaContainer;
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private OrderActionConsumer _consumer;
    private readonly Fixture _fixture = new();

    public OrderActionConsumerTests()
    {
        _kafkaContainer = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka:latest")
            .WithExposedPort(9092)
            .Build();
    }
    
    public async Task InitializeAsync()
    {
        await _kafkaContainer.StartAsync();

        _consumer = IntegrationTestsEntitiesFactory.CreateTestOrderActionsConsumer(_kafkaContainer, _emailServiceMock);
    }

    public async Task DisposeAsync()
    {
        await _kafkaContainer.StopAsync();
    }
    
    [Fact]
    public async Task ConsumeMessages_WhenCalled_ShouldNotifyUser()
    {
        //Arrange
        var order = _fixture.Create<ConsumedOrder>();
        
        using var producer = IntegrationTestsEntitiesFactory.CreateTestProducer<ConsumedOrder>(_kafkaContainer);
        
        await producer.ProduceAsync("order-actions", new Message<Null, ConsumedOrder>
        {
            Value = order
        });
        
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));
        
        //Act
        Task.Run(() => _consumer.ConsumeMessagesAsync(cancellationTokenSource.Token));
        
        await Task.Delay(2000);
        
        //Assert
        _emailServiceMock.Verify(emailService => emailService.SendOrderStatusNotificationAsync(It.IsAny<ConsumedOrder>()), Times.Once);
    }
}