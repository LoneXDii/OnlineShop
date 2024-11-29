using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Products;

internal class ProductIncludeDiscountSpecification : CombinableSpecification<Product>
{
    public ProductIncludeDiscountSpecification() : base()
    {
        AddInclude(p => p.Discount);
    }
}
