using AutoFixture;
using Testcontainers.Kafka;
using UserService.DAL.Entities;
using UserService.DAL.Models;
using UserService.DAL.Services.MessageBrocker.ProducerService;
using UserService.Tests.Factories;

namespace UserService.Tests.IntegrationTests.Services;

public class ProducerServiceTests: IAsyncLifetime
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
    public async Task ProduceUserCreationAsync_WhenCalled_ShouldProduceUserToKafka()
    {
        //Arrange
        var user = _fixture.Create<AppUser>();
        
        //Act
        await _producerService.ProduceUserCreationAsync(user);
        
        //Assert
        using var consumer = IntegrationTestsEntitiesFactory.CreateTestConsumer<MqUserRequest>(_kafkaContainer);
        
        consumer.Subscribe("user-creation");
        var result = consumer.Consume(TimeSpan.FromSeconds(10));
        
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Message.Value.Id);
        Assert.Equal(user.Email, result.Message.Value.Email);
        Assert.Equal($"{user.FirstName} {user.LastName}", result.Message.Value.Name);
    }
}