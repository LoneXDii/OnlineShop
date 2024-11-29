using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Products;

internal class ProductIncludesSpecification : CombinableSpecification<Product>
{
    public ProductIncludesSpecification() : base()
    {
        AddInclude(p => p.Categories);
        AddInclude(p => p.Discount);
    }
}
