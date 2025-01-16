using ProductsService.Domain.Entities;
using System.Linq.Expressions;

namespace ProductsService.Application.Specifications.Products;

internal class ProductCategorySpecification : Specification<Product>
{
    private readonly int? _categoryId;

    public ProductCategorySpecification(int? categoryId)
    {
        _categoryId = categoryId;
    }

    public override Expression<Func<Product, bool>> ToExpression()
    {
        if (_categoryId is null)
        {
            return product => true;
        }

        return product => product.Categories.Any(category => category.Id == _categoryId && category.ParentId == null);
    }
}
