using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Products;

internal class ProductMaxPriceSpecification : CombinableSpecification<Product>
{
    public ProductMaxPriceSpecification(double maxPrice)
        : base(p => p.Price <= maxPrice)
    { }
}
