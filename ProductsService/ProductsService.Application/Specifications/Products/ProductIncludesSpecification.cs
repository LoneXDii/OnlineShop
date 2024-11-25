using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Products;

internal class ProductIncludesSpecification : CombinableSpecification<Product>
{
    public ProductIncludesSpecification()
        : base()
    {
        AddInclude($"{nameof(Product.ProductAttributes)}.{nameof(ProductAttribute.Attribute)}");
        AddInclude(p => p.Category);
    }
}
