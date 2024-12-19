using Confluent.Kafka;
using OrderService.Domain.Abstractions.Data;
using OrderService.Infrastructure.Models;
using OrderService.Infrastructure.Services.MessageBrocker.Serializers;

namespace OrderService.Infrastructure.Services.MessageBrocker;

internal class ProducerService : IProducerService
{
    private readonly ProducerConfig _producerConfig;

    public ProducerService(ProducerConfig producerConfig)
    {
        _producerConfig = producerConfig;
    }

    public async Task ProduceUserStripeIdAsync(string userId, string stripeId, 
        CancellationToken  cancellationToken = default)
    {
        using var producer = new ProducerBuilder<Null, UserStripeIdDTO>(_producerConfig)
            .SetValueSerializer(new UserStripeIdDtoSerializer())
            .Build();

        var message = new UserStripeIdDTO 
        { 
            StripeId = stripeId, 
            UserId = userId 
        };

        await producer.ProduceAsync(topic: "user-stripe-id-creation",
            new Message<Null, UserStripeIdDTO> { Value = message },
            cancellationToken);

        producer.Flush(cancellationToken);
    }
}
