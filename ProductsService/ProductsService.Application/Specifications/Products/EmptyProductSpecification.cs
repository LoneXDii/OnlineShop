using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Products;

internal class EmptyProductSpecification : CombinableSpecification<Product>
{
    public EmptyProductSpecification()
        : base()
    {
        AddInclude($"{nameof(Product.ProductAttributes)}.{nameof(ProductAttribute.Attribute)}");
        AddInclude(p => p.Category);
    }
}
