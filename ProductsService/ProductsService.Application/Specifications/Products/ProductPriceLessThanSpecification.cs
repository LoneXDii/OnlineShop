using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Products;

internal class ProductPriceLessThanSpecification : CombinableSpecification<Product>
{
    public ProductPriceLessThanSpecification(double price)
        : base(product => product.Price <= price) { }
}
