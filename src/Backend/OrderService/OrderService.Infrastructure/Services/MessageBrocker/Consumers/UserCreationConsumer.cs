using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Abstractions.Payments;
using OrderService.Infrastructure.Models;
using OrderService.Infrastructure.Services.MessageBrocker.Serialization;

namespace OrderService.Infrastructure.Services.MessageBrocker.Consumers;

internal class UserCreationConsumer : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<UserCreationConsumer> _logger;

    public UserCreationConsumer(ConsumerConfig consumerConfig, IServiceScopeFactory serviceScopeFactory, 
        ILogger<UserCreationConsumer> logger)
    {
        _consumerConfig = consumerConfig;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("User creation consumer started");

        Task.Run(() => ConsumeMessagesAsync(stoppingToken));

        return Task.CompletedTask;
    }

    internal async Task ConsumeMessagesAsync(CancellationToken cancellationToken)
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

            var stripeId = await paymentService.CreateCustomerAsync(consumeResult.Message.Value.Email, consumeResult.Message.Value.Name);

            _logger.LogInformation($"Consumed user: {consumeResult.Message.Value.Email}, generated stripeId: {stripeId}");

            await producerService.ProduceUserStripeIdAsync(consumeResult.Message.Value.Id, stripeId, cancellationToken);
        }

        _logger.LogInformation("User creation consumer stopped");
    }
}
