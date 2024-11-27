using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Products;

internal class ProductMinPriceSpecification : CombinableSpecification<Product>
{
    public ProductMinPriceSpecification(double minPrice)
        : base(p => p.Price >= minPrice)
    { }
}
