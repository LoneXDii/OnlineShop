using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Products;

internal class ProductIncludeCategoriesSpecification : CombinableSpecification<Product>
{
    public ProductIncludeCategoriesSpecification() : base()
    {
        AddInclude(p => p.Categories);
    }
}
