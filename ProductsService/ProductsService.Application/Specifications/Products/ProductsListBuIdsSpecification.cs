using ProductsService.Domain.Entities;
using System.Linq.Expressions;

namespace ProductsService.Application.Specifications.Products;

internal class ProductsListBuIdsSpecification : Specification<Product>
{
    private readonly IEnumerable<int> _ids;

    public ProductsListBuIdsSpecification(IEnumerable<int> ids)
    {
        _ids = ids;
    }

    public override Expression<Func<Product, bool>> ToExpression()
    {
        return product => _ids.Contains(product.Id);
    }
}
