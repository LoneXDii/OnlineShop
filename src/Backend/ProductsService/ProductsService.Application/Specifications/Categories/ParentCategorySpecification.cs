using ProductsService.Domain.Entities;
using System.Linq.Expressions;

namespace ProductsService.Application.Specifications.Categories;

internal class ParentCategorySpecification : Specification<Category>
{
    private readonly int _parentId;

    public ParentCategorySpecification(int parentId)
    {
        _parentId = parentId;
    }

    public override Expression<Func<Category, bool>> ToExpression()
    {
        return category => category.ParentId == _parentId;
    }
}
