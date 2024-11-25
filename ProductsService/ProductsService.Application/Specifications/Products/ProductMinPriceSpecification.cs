using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Products;

internal class ProductMinPriceSpecification : CombinableSpecification<Product>
{
    public ProductMinPriceSpecification(double price) 
        : base(product => product.Price >= price)
    { }
}
