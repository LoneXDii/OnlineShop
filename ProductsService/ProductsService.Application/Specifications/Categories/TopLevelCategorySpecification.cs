using ProductsService.Domain.Entities;
using System.Linq.Expressions;

namespace ProductsService.Application.Specifications.Categories;

internal class TopLevelCategorySpecification : Specification<Category>
{
    public TopLevelCategorySpecification() { }

    public override Expression<Func<Category, bool>> ToExpression()
    {
        return category => category.ParentId == null;
    }
}
