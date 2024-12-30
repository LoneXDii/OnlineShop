using ProductsService.Domain.Entities;

namespace ProductsService.Domain.Abstractions.MessageBrocker;

public interface IProducerService
{
    Task ProduceProductCreationAsync(Product product, CancellationToken cancellationToken = default);
}
