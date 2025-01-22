using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Infrastructure.Models;
using ProductsService.Infrastructure.Services.MessageBrocker.Serialization;

namespace ProductsService.Infrastructure.Services.MessageBrocker.Consumers;

internal class PriceIdConsumer : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<PriceIdConsumer> _logger;

    public PriceIdConsumer(ConsumerConfig consumerConfig, IServiceScopeFactory serviceScopeFactory, 
        ILogger<PriceIdConsumer> logger)
    {
        _consumerConfig = consumerConfig;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Price id consumer started");

        Task.Run(() => ConsumeMessages(stoppingToken));

        return Task.CompletedTask;
    }

    public async Task ConsumeMessages(CancellationToken cancellationToken)
    {
        //Need this because PriceIdConsumer is a singletone service, while UnitOfWork is scoped
        using var scope = _serviceScopeFactory.CreateScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        using var consumer = new ConsumerBuilder<Ignore, ConsumedPriceId>(_consumerConfig)
            .SetValueDeserializer(new KafkaDeserializer<ConsumedPriceId>())
            .Build();

        consumer.Subscribe("product-price-id-creation");

        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5));

            if (consumeResult is null)
            {
                continue;
            }

            var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(consumeResult.Message.Value.Id, cancellationToken);

            if (product is null)
            {
                _logger.LogError($"No product with id: {consumeResult.Message.Value.Id} found");

                continue;
            }

            product.PriceId = consumeResult.Message.Value.PriceId;
            _logger.LogInformation($"Consumed priceId: {product.PriceId} for product with id: {product.Id}");

            await unitOfWork.ProductCommandRepository.UpdateAsync(product, cancellationToken);
        }

        _logger.LogInformation("Price id consumer stopped");
    }
}
