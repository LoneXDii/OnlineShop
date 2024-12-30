using OrderService.Domain.Entities;

namespace OrderService.Domain.Abstractions.Data;

public interface IProductService
{
    Task<ProductEntity?> GetByIdIfSufficientQuantityAsync(int id, int quantity, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductEntity>?> TakeProductsIfSufficientQuantityAsync(IEnumerable<ProductEntity> products, 
        CancellationToken cancellationToken = default);
    Task ReturnProductsAsync(IEnumerable<ProductEntity> products, CancellationToken cancellationToken = default);
}
