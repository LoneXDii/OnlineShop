using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Products;

internal class ProductPriceGreaterThanSpecification : CombinableSpecification<Product>
{
    public ProductPriceGreaterThanSpecification(double price) 
        : base(product => product.Price >= price)
    {
        AddInclude($"{nameof(Product.ProductAttributes)}.{nameof(ProductAttribute.Attribute)}");
        AddInclude(p => p.Category);
    }
}
