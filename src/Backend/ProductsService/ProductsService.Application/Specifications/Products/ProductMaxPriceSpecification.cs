using ProductsService.Domain.Entities;
using System.Linq.Expressions;

namespace ProductsService.Application.Specifications.Products;

internal class ProductMaxPriceSpecification : Specification<Product>
{
    private readonly double? _maxPrice;

    public ProductMaxPriceSpecification(double? maxPrice)
    {
        _maxPrice = maxPrice;
    }

    public override Expression<Func<Product, bool>> ToExpression()
    {
        if (_maxPrice is null)
        {
            return product => true;
        }

        return product => product.Price <= _maxPrice;
    }
}
