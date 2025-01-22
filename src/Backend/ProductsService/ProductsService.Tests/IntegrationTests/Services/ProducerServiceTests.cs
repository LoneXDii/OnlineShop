using ProductsService.Domain.Abstractions.MessageBrocker;
using ProductsService.Tests.Factories;
using Testcontainers.Kafka;

namespace ProductsService.Tests.IntegrationTests.Services;

public class ProducerServiceTests : IAsyncLifetime
{
    private readonly KafkaContainer _kafkaContainer;
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
    public async Task ProduceProductCreationAsync_WhenCalled_ShouldProduceMessageToKafka()
    {
        //Arrange
        var product = EntityFactory.CreateProduct();
        
        //Act
        await _producerService.ProduceProductCreationAsync(product);

        //Assert
        using var consumer = IntegrationTestsEntitiesFactory.CreateTestConsumer(_kafkaContainer);

        consumer.Subscribe("product-creation");

        var cr = consumer.Consume(TimeSpan.FromSeconds(10));

        Assert.NotNull(cr);
        Assert.Equal(product.Id, cr.Message.Value.Id);
        Assert.Equal(product.Name, cr.Message.Value.Name);
    }
}