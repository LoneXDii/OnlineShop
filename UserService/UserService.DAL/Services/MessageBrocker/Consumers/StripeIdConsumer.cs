using Confluent.Kafka;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserService.DAL.Entities;
using UserService.DAL.Models;
using UserService.DAL.Services.MessageBrocker.Serialization;

namespace UserService.DAL.Services.MessageBrocker.Consumers;

internal class StripeIdConsumer : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<StripeIdConsumer> _logger;

    public StripeIdConsumer(ConsumerConfig consumerConfig, IServiceScopeFactory serviceScopeFactory,
        ILogger<StripeIdConsumer> logger)
    {
        _consumerConfig = consumerConfig;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Stripe id consumer started");

        Task.Run(() => ConsumeMessages(stoppingToken));

        return Task.CompletedTask;
    }

    private async Task ConsumeMessages(CancellationToken cancellationToken)
    {
        //Need this because StripeIdConsumer is a singletone service, while UserManager is scoped
        using var scope = _serviceScopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        using var consumer = new ConsumerBuilder<Ignore, ConsumedStripeId>(_consumerConfig)
             .SetValueDeserializer(new KafkaDeserializer<ConsumedStripeId>())
             .Build();

        consumer.Subscribe("user-stripe-id-creation");

        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5));

            if (consumeResult is null)
            {
                continue;
            }

            var user = await userManager.FindByIdAsync(consumeResult.Value.UserId);

            if (user is null)
            {
                _logger.LogError($"Consumed stripeId: {user.StripeId} for userId: {user.Id} but user does not exist");

                continue;
            }

            user.StripeId = consumeResult.Message.Value.StripeId;
            _logger.LogInformation($"Consumed stripeId: {user.StripeId} for userId: {user.Id}");

            await userManager.UpdateAsync(user);
        }

        _logger.LogInformation("Stripe id consumer stopped");
    }
}
