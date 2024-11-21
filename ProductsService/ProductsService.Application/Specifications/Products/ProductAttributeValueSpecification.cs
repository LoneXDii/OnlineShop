using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Products;

internal class ProductAttributeValueSpecification : CombinableSpecification<Product>
{
    public ProductAttributeValueSpecification(string attributeName, string attributeValue)
        : base(p => p.ProductAttributes.FirstOrDefault(pa => pa.Attribute.Name == attributeName).Value == attributeValue) 
    {
		AddInclude($"{nameof(Product.ProductAttributes)}.{nameof(ProductAttribute.Attribute)}");
		AddInclude(p => p.Category);
	}
}
