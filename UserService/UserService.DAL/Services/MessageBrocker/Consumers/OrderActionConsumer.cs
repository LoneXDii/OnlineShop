using Confluent.Kafka;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserService.DAL.Models;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.MessageBrocker.Serialization;

namespace UserService.DAL.Services.MessageBrocker.Consumers;

internal class OrderActionConsumer : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<OrderActionConsumer> _logger;

    public OrderActionConsumer(ConsumerConfig consumerConfig, IServiceScopeFactory serviceScopeFactory, 
        ILogger<OrderActionConsumer> logger)
    {
        _consumerConfig = consumerConfig;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Order action consumer started");

        Task.Run(() => ConsumeMessages(stoppingToken));

        return Task.CompletedTask;
    }

    private async Task ConsumeMessages(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        using var consumer = new ConsumerBuilder<Ignore, ConsumedOrder>(_consumerConfig)
             .SetValueDeserializer(new KafkaDeserializer<ConsumedOrder>())
             .Build();

        consumer.Subscribe("order-actions");

        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5));

            if (consumeResult is null)
            {
                continue;
            }

            _logger.LogInformation($"Consumed order {consumeResult.Message.Value.Id}");

            await emailService.SendOrderStatusNotificationAsync(consumeResult.Message.Value);
        }

        _logger.LogInformation("Order action consumer finished");
    }
}
