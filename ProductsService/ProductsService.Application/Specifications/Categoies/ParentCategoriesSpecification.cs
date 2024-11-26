using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Categories;

internal class ParentCategoriesSpecification : CombinableSpecification<Category>
{
    public ParentCategoriesSpecification()
        : base(category => category.ParentId == null)
    {

    }
}
