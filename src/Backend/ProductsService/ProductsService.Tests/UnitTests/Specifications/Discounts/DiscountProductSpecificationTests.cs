using ProductsService.Application.Specifications.Discounts;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UnitTests.Specifications.Discounts;

public class DiscountProductSpecificationTests
{
    private readonly DiscountProductSpecification _specification = new DiscountProductSpecification(1);

    [Fact]
    public void IsSatisfiedBy_WhenDiscountProductIdMatches_ShouldReturnTrue()
    {
        //Arrange
        var discount = new Discount { ProductId = 1 };

        //Act
        var result = _specification.IsSatisfiedBy(discount);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_WhenDiscountProductIdDoesNotMatch_ShouldReturnFalse()
    {
        //Arrange
        var discount = new Discount { ProductId = 2 };

        //Act
        var result = _specification.IsSatisfiedBy(discount);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void ToExpression_WhenCalled_ShouldReturnCorrectExpression()
    {
        //Arrange
        var expression = _specification.ToExpression();

        //Act
        var func = expression.Compile();
        var matchingDiscount = new Discount { ProductId = 1 };
        var nonMatchingDiscount = new Discount { ProductId = 3 };

        //Assert
        Assert.True(func(matchingDiscount));
        Assert.False(func(nonMatchingDiscount));
    }
}