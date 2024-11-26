namespace ProductsService.Application.Specifications.Attributes;
using Attribute = ProductsService.Domain.Entities.Attribute;

internal class AttributeCategorySpecification : CombinableSpecification<Attribute>
{
    public AttributeCategorySpecification(int categoryId)
        : base(attribute => attribute.CategoryId == categoryId)
    { }
}
