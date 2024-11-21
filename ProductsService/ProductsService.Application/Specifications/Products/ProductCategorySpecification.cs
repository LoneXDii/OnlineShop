using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Products;

internal class ProductCategorySpecification : CombinableSpecification<Product>
{
    public ProductCategorySpecification(int categoryId) 
        : base(product => product.CategoryId == categoryId)
	{
		AddInclude($"{nameof(Product.ProductAttributes)}.{nameof(ProductAttribute.Attribute)}");
		AddInclude(p => p.Category);
	}
}
