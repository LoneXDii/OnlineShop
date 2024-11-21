using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Products;

internal class ProductPriceGreaterThenSpecification : CombinableSpecification<Product>
{
    public ProductPriceGreaterThenSpecification(double price) 
        : base(product => product.Price >= price)
	{
		AddInclude($"{nameof(Product.ProductAttributes)}.{nameof(ProductAttribute.Attribute)}");
		AddInclude(p => p.Category);
	}
}
