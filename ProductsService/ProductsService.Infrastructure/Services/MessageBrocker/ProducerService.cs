using AutoMapper;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using ProductsService.Domain.Abstractions.MessageBrocker;
using ProductsService.Domain.Entities;
using ProductsService.Infrastructure.Models;
using ProductsService.Infrastructure.Services.MessageBrocker.Serialization;

namespace ProductsService.Infrastructure.Services.MessageBrocker;

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

    public async Task ProduceProductCreationAsync(Product product, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Producing creation of product with id: {product.Id}");

        using var producer = new ProducerBuilder<Null, ProductCreationDTO>(_producerConfig)
            .SetValueSerializer(new KafkaSerializer<ProductCreationDTO>())
            .Build();

        var message = _mapper.Map<ProductCreationDTO>(product);

        await producer.ProduceAsync(topic: "product-creation",
            new Message<Null, ProductCreationDTO> { Value = message },
            cancellationToken);

        producer.Flush();
    }
}
