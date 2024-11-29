using ProductsService.Domain.Entities;

namespace ProductsService.Application.Specifications.Discounts;

internal class DiscountProductSpecification : CombinableSpecification<Discount>
{
    public DiscountProductSpecification(int productId)
        : base(discount => discount.ProductId == productId)
    { }
}
