using ProductsService.Domain.Entities;
using System.Linq.Expressions;

namespace ProductsService.Application.Specifications.Categories;

internal class CategoriesByIdsArraySpecification : Specification<Category>
{
    private readonly int[] _ids;

    public CategoriesByIdsArraySpecification(int[] ids)
    {
        _ids = ids;
    }

    public override Expression<Func<Category, bool>> ToExpression()
    {
        return category => _ids.Contains(category.Id);
    }
}
