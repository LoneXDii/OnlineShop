using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Categoies;

internal class CategoryAttributesValuesSpecification : CombinableSpecification<Category>
{
    public CategoryAttributesValuesSpecification(int categoryId)
        : base(category => category.ParentId == categoryId)
    {
        AddInclude(category => category.Children);
    }
}
