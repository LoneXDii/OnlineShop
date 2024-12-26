using AutoMapper;
using Confluent.Kafka;
using UserService.DAL.Entities;
using UserService.DAL.Models;
using UserService.DAL.Services.MessageBrocker.Serialization;

namespace UserService.DAL.Services.MessageBrocker.ProducerService;

internal class ProducerService : IProducerService
{
    private readonly ProducerConfig _producerConfig;
    private readonly IMapper _mapper;

    public ProducerService(ProducerConfig producerConfig, IMapper mapper)
    {
        _producerConfig = producerConfig;
        _mapper = mapper;
    }

    public async Task ProduceUserCreationAsync(AppUser user, CancellationToken cancellationToken = default)
    {
        using var producer = new ProducerBuilder<Null, MqUserRequest>(_producerConfig)
            .SetValueSerializer(new KafkaSerializer<MqUserRequest>())
            .Build();

        var request = _mapper.Map<MqUserRequest>(user);

        await producer.ProduceAsync(topic: "user-creation",
            new Message<Null, MqUserRequest> { Value = request },
            cancellationToken);

        producer.Flush(cancellationToken);
    }
}
