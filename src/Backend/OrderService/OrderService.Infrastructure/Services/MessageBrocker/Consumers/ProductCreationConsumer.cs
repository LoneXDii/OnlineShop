using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Abstractions.Payments;
using OrderService.Infrastructure.Models;
using OrderService.Infrastructure.Services.MessageBrocker.Serialization;

namespace OrderService.Infrastructure.Services.MessageBrocker.Consumers;

internal class ProductCreationConsumer : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<ProductCreationConsumer> _logger;

    public ProductCreationConsumer(ConsumerConfig consumerConfig, IServiceScopeFactory serviceScopeFactory,
        ILogger<ProductCreationConsumer> logger)
    {
        _consumerConfig = consumerConfig;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Poduct creation consumer started");

        Task.Run(() => ConsumeMessages(stoppingToken));

        return Task.CompletedTask;
    }

    private async Task ConsumeMessages(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
        var producerService = scope.ServiceProvider.GetRequiredService<IProducerService>();

        using var consumer = new ConsumerBuilder<Ignore, ConsumedProduct>(_consumerConfig)
             .SetValueDeserializer(new KafkaDeserializer<ConsumedProduct>())
             .Build();

        consumer.Subscribe("product-creation");

        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5));

            if (consumeResult is null)
            {
                continue;
            }

            var priceId = await paymentService.CreateProductAsync(consumeResult.Message.Value.Name, consumeResult.Message.Value.Price);

            _logger.LogInformation($"Consumed product: {consumeResult.Message.Value.Name}, generated priceId: {priceId}");

            await producerService.ProduceProductPriceIdAsync(consumeResult.Message.Value.Id, priceId, cancellationToken);
        }

        _logger.LogInformation("Poduct creation consumer stopped");
    }
}
