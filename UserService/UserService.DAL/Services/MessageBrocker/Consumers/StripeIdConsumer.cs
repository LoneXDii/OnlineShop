using Confluent.Kafka;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.DAL.Entities;
using UserService.DAL.Models;
using UserService.DAL.Services.MessageBrocker.Deserealizers;

namespace UserService.DAL.Services.MessageBrocker.Consumers;

internal class StripeIdConsumer : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public StripeIdConsumer(ConsumerConfig consumerConfig, IServiceScopeFactory serviceScopeFactory)
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
        //Need this because StripeIdConsumer is a singletone service, while UserManager is scoped
        using var scope = _serviceScopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        using var consumer = new ConsumerBuilder<Ignore, ConsumedStripeId>(_consumerConfig)
             .SetValueDeserializer(new ConsumedStripeIdDeserealizer())
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
                continue;
            }

            user.StripeId = consumeResult.Value.StripeId;

            await userManager.UpdateAsync(user);
        }
    }
}
