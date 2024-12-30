using OrderService.Domain.Entities;

namespace OrderService.Domain.Abstractions.Data;

public interface IProducerService
{
    Task ProduceUserStripeIdAsync(string userId, string stripeId, CancellationToken cancellationToken = default);
    Task ProduceProductPriceIdAsync(int productId, string priceId, CancellationToken cancellationToken = default);
    Task ProduceOrderActionsAsync(OrderEntity order, CancellationToken cancellationToken = default);
}
