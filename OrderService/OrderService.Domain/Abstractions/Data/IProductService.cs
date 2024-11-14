using OrderService.Domain.Entities;

namespace OrderService.Domain.Abstractions.Data;

public interface IProductService
{
	Task<Product?> GetByIdIfSufficientQuantityAsync(int id, int quantity);
	Task<IEnumerable<Product>?> TakeProductsIfSufficientQuantityAsync(IEnumerable<Product> products);
	Task ReturnProductsAsync(IEnumerable<Product> products);
}
