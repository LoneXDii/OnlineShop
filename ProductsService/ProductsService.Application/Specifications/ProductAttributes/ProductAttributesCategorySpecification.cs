using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.ProductAttributes;

internal class ProductAttributesCategorySpecification : CombinableSpecification<ProductAttribute>
{
    public ProductAttributesCategorySpecification(int categoryId)
        : base(pa => pa.Attribute.CategoryId == categoryId)
    {
        Includes.Add(pa => pa.Attribute);
    }
}
