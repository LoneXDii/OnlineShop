using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.ProductAttributes;

internal class UniqueProductAttributeValueSpecification : CombinableSpecification<ProductAttribute>
{
    public UniqueProductAttributeValueSpecification() : base()
    {
        DescendingBy = pa => new { pa.AttributeId, pa.Value };
    }
}
