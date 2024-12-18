
using Confluent.Kafka;

namespace UserService.DAL.Services.MessageBrocker.ProducerService;

internal class ProducerService : IProducerService
{
    private readonly ProducerConfig _producerConfig;

    public ProducerService(ProducerConfig producerConfig)
    {
        _producerConfig = producerConfig;
    }

    public async Task ProduceAsync(CancellationToken cancellationToken = default)
    {
        using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();

        var deliveryResult = await producer.ProduceAsync(topic: "test-topic",
            new Message<Null, string> { Value = $"Hello, Kafka! {DateTime.UtcNow}"}, 
            cancellationToken);

        Console.WriteLine($"Delivered message to {deliveryResult.Value}, Offset: {deliveryResult.Offset}");

        producer.Flush(cancellationToken);
    }
}
