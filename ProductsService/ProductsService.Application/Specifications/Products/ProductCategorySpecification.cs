using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Products;

internal class ProductCategorySpecification : CombinableSpecification<Product>
{
    public ProductCategorySpecification(int categoryId)
        : base(p => p.CategoryProducts.Any(c => c.Category.Id == categoryId))
    { }
}
