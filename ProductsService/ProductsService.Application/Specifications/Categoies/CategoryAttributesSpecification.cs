using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Categoies;

internal class CategoryAttributesSpecification : CombinableSpecification<Category>
{
    public CategoryAttributesSpecification(int categoryId)
        : base(category => category.ParentId == categoryId)
    {

    }
}
