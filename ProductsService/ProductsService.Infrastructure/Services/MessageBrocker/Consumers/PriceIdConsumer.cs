using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Infrastructure.Models;
using ProductsService.Infrastructure.Services.MessageBrocker.Serialization;

namespace ProductsService.Infrastructure.Services.MessageBrocker.Consumers;

internal class PriceIdConsumer : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PriceIdConsumer(ConsumerConfig consumerConfig, IServiceScopeFactory serviceScopeFactory)
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
                continue;
            }

            product.PriceId = consumeResult.Value.PriceId;

            await unitOfWork.ProductCommandRepository.UpdateAsync(product, cancellationToken);
        }
    }
}
