using AutoFixture;
using Confluent.Kafka;
using Microsoft.AspNetCore.Identity;
using Moq;
using Testcontainers.Kafka;
using UserService.DAL.Entities;
using UserService.DAL.Models;
using UserService.DAL.Services.MessageBrocker.Consumers;
using UserService.Tests.Factories;

namespace UserService.Tests.IntegrationTests.Services;

public class StripeIdConsumerTests : IAsyncLifetime
{
    private readonly KafkaContainer _kafkaContainer;
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private StripeIdConsumer _consumer;
    private readonly Fixture _fixture = new();

    public StripeIdConsumerTests()
    {
        _kafkaContainer = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka:latest")
            .WithExposedPort(9092)
            .Build();

        _userManagerMock = MocksFactory.CreateUserManager();
    }
    
    public async Task InitializeAsync()
    {
        await _kafkaContainer.StartAsync();

        _consumer = IntegrationTestsEntitiesFactory.CreateTestStripeIdConsumer(_kafkaContainer, _userManagerMock);
    }

    public async Task DisposeAsync()
    {
        await _kafkaContainer.StopAsync();
    }
    
    [Fact]
    public async Task ConsumeMessages_WhenCalled_ShouldUpdateUser()
    {
        //Arrange
        var consumedStripeId = _fixture.Create<ConsumedStripeId>();
        var user = _fixture.Create<AppUser>();
        
        _userManagerMock.Setup(u => u.FindByIdAsync(consumedStripeId.UserId)).ReturnsAsync(user);
        
        using var producer = IntegrationTestsEntitiesFactory.CreateTestProducer<ConsumedStripeId>(_kafkaContainer);
        
        await producer.ProduceAsync("user-stripe-id-creation", new Message<Null, ConsumedStripeId>
        {
            Value = consumedStripeId
        });
        
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));
        
        //Act
        Task.Run(() => _consumer.ConsumeMessagesAsync(cancellationTokenSource.Token));
        
        await Task.Delay(2000);
        
        //Assert
        Assert.Equal(consumedStripeId.StripeId, user.StripeId);
        _userManagerMock.Verify(manager => manager.UpdateAsync(user), Times.Once);
    }
}