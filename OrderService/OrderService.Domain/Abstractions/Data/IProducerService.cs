namespace OrderService.Domain.Abstractions.Data;

public interface IProducerService
{
    Task ProduceUserStripeIdAsync(string userId, string stripeId, CancellationToken cancellationToken = default);
}
