using ProductsService.Domain.Entities;
using System.Linq.Expressions;

namespace ProductsService.Application.Specifications.Products;

internal class ProductMinPriceSpecification : Specification<Product>
{
    private readonly double? _minPrice;

    public ProductMinPriceSpecification(double? minPrice)
    {
        _minPrice = minPrice;
    }

    public override Expression<Func<Product, bool>> ToExpression()
    {
        if (_minPrice is null)
        {
            return product => true;
        }

        return product => product.Price >= _minPrice;
    }
}
