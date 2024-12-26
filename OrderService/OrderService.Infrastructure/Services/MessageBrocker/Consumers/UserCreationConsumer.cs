using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Abstractions.Payments;
using OrderService.Infrastructure.Models;
using OrderService.Infrastructure.Services.MessageBrocker.Serialization;

namespace OrderService.Infrastructure.Services.MessageBrocker.Consumers;

internal class UserCreationConsumer : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UserCreationConsumer(ConsumerConfig consumerConfig, IServiceScopeFactory serviceScopeFactory)
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
        //Need this because UserCreationConsumer is a singletone service, while PaymentService and ProducerServiec are scoped
        using var scope = _serviceScopeFactory.CreateScope();

        var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
        var producerService = scope.ServiceProvider.GetRequiredService<IProducerService>();

        using var consumer = new ConsumerBuilder<Ignore, ConsumedUser>(_consumerConfig)
             .SetValueDeserializer(new KafkaDeserializer<ConsumedUser>())
             .Build();

        consumer.Subscribe("user-creation");

        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5));

            if (consumeResult is null)
            {
                continue;
            }

            var stipeId = await paymentService.CreateCustomerAsync(consumeResult.Value.Email, consumeResult.Value.Name);

            await producerService.ProduceUserStripeIdAsync(consumeResult.Value.Id, stipeId, cancellationToken);
        }
    }
}
