using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Abstractions.Payments;
using OrderService.Infrastructure.Models;
using OrderService.Infrastructure.Services.MessageBrocker.Deserializers;

namespace OrderService.Infrastructure.Services.MessageBrocker.Consumers;

internal class ProductCreationConsumer : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ProductCreationConsumer(ConsumerConfig consumerConfig, IServiceScopeFactory serviceScopeFactory)
    {
        _consumerConfig = consumerConfig;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Task.Run(() => ConsumeMessages(stoppingToken));

        return Task.CompletedTask;
    }

    private async Task ConsumeMessages(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
        var producerService = scope.ServiceProvider.GetRequiredService<IProducerService>();

        using var consumer = new ConsumerBuilder<Ignore, ConsumedProduct>(_consumerConfig)
             .SetValueDeserializer(new ConsumedProductDeserializer())
             .Build();

        consumer.Subscribe("product-creation");

        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5));

            if (consumeResult is null)
            {
                continue;
            }

            var priceId = await paymentService.CreateProductAsync(consumeResult.Value.Name, consumeResult.Value.Price);

            await producerService.ProduceProductPriceIdAsync(consumeResult.Value.Id, priceId, cancellationToken);
        }
    }
}
