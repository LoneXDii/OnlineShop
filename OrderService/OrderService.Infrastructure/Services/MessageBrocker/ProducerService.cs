using AutoMapper;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Models;
using OrderService.Infrastructure.Services.MessageBrocker.Serialization;
using System.Security.Claims;

namespace OrderService.Infrastructure.Services.MessageBrocker;

internal class ProducerService : IProducerService
{
    private readonly ProducerConfig _producerConfig;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProducerService(ProducerConfig producerConfig, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _producerConfig = producerConfig;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task ProduceUserStripeIdAsync(string userId, string stripeId, 
        CancellationToken  cancellationToken = default)
    {
        using var producer = new ProducerBuilder<Null, UserStripeIdDTO>(_producerConfig)
            .SetValueSerializer(new KafkaSerializer<UserStripeIdDTO>())
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

    public async Task ProduceProductPriceIdAsync(int productId, string priceId, CancellationToken cancellationToken = default)
    {
        using var producer = new ProducerBuilder<Null, ProductPriceIdDTO>(_producerConfig)
            .SetValueSerializer(new KafkaSerializer<ProductPriceIdDTO>())
            .Build();

        var message = new ProductPriceIdDTO 
        { 
            Id = productId,
            PriceId = priceId
        };

        await producer.ProduceAsync(topic: "product-price-id-creation",
            new Message<Null, ProductPriceIdDTO> { Value = message },
            cancellationToken);

        producer.Flush(cancellationToken);
    }

    public async Task ProduceOrderActionsAsync(OrderEntity order, CancellationToken cancellationToken = default)
    {
        using var producer = new ProducerBuilder<Null, ProducedOrderDTO>(_producerConfig)
            .SetValueSerializer(new KafkaSerializer<ProducedOrderDTO>())
            .Build();

        var message = _mapper.Map<ProducedOrderDTO>(order);

        message.UserEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

        await producer.ProduceAsync(topic: "order-actions",
            new Message<Null, ProducedOrderDTO> { Value = message },
            cancellationToken);

        producer.Flush(cancellationToken);
    }
}
