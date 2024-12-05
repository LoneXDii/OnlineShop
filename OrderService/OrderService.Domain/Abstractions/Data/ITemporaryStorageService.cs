using OrderService.Domain.Entities;

namespace OrderService.Domain.Abstractions.Data;

public interface ITemporaryStorageService
{
    Task  SaveCartAsync(Dictionary<int, ProductEntity> cart, CancellationToken cancellationToken = default);
    Task<Dictionary<int, ProductEntity>> GetCartAsync(CancellationToken cancellationToken = default);
}
