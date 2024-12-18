using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace OrderService.Infrastructure.Services;

internal class ConsumerService : IHostedService
{
    private readonly ConsumerConfig _consumerConfig;

    public ConsumerService(ConsumerConfig consumerConfig)
    {
        _consumerConfig = consumerConfig;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        Task.Run(() => ConsumeMessages(stoppingToken));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private Task ConsumeMessages(CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();

        consumer.Subscribe("test-topic");

        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5));

            if (consumeResult is null)
            {
                continue;
            }

            Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.Offset}'");
        }

        return Task.CompletedTask;
    }
}
