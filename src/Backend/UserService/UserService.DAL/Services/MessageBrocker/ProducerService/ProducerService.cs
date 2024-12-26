using AutoMapper;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using UserService.DAL.Entities;
using UserService.DAL.Models;
using UserService.DAL.Services.MessageBrocker.Serialization;

namespace UserService.DAL.Services.MessageBrocker.ProducerService;

internal class ProducerService : IProducerService
{
    private readonly ProducerConfig _producerConfig;
    private readonly IMapper _mapper;
    private readonly ILogger<ProducerService> _logger;

    public ProducerService(ProducerConfig producerConfig, IMapper mapper, ILogger<ProducerService> logger)
    {
        _producerConfig = producerConfig;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task ProduceUserCreationAsync(AppUser user, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Producing creation of user with id: {user.Id}");

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
