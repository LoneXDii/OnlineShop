using OrderService.Domain.Entities;

namespace OrderService.Domain.Abstractions.Data;

public interface IProductService
{
	Task<ProductEntity?> GetByIdIfSufficientQuantityAsync(int id, int quantity);
	Task<IEnumerable<ProductEntity>?> TakeProductsIfSufficientQuantityAsync(IEnumerable<ProductEntity> products);
	Task ReturnProductsAsync(IEnumerable<ProductEntity> products);
}
