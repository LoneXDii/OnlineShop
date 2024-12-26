using ProductsService.Domain.Entities;
using System.Linq.Expressions;

namespace ProductsService.Application.Specifications.Discounts;

internal class DiscountProductSpecification : Specification<Discount>
{
    private readonly int _productId;

    public DiscountProductSpecification(int productId)
    {
        _productId = productId;
    }

    public override Expression<Func<Discount, bool>> ToExpression()
    {
        return discount => discount.ProductId == _productId;
    }
}
