using ProductsService.Application.Specifications.Products;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UnitTests.Specifications.Products;

public class ProductMinPriceSpecificationTests
{
    [Fact]
    public void IsSatisfiedBy_WhenMinPriceIsNull_ShouldReturnTrue()
    {
        //Arrange
        var specification = new ProductMinPriceSpecification(null);
        var product = new Product { Price = 100 };

        //Act
        var result = specification.IsSatisfiedBy(product);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_WhenProductPriceIsGreaterThanMinPrice_ShouldReturnTrue()
    {
        //Arrange
        var specification = new ProductMinPriceSpecification(50);
        var product = new Product { Price = 100 };

        //Act
        var result = specification.IsSatisfiedBy(product);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_WhenProductPriceIsEqualToMinPrice_ShouldReturnTrue()
    {
        //Arrange
        var specification = new ProductMinPriceSpecification(50);
        var product = new Product { Price = 50 };

        //Act
        var result = specification.IsSatisfiedBy(product);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_WhenProductPriceIsLessThanMinPrice_ShouldReturnFalse()
    {
        //Arrange
        var specification = new ProductMinPriceSpecification(100);
        var product = new Product { Price = 50 };

        //Act
        var result = specification.IsSatisfiedBy(product);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void ToExpression_WhenCalled_ShouldReturnCorrectExpression()
    {
        //Arrange
        var specification = new ProductMinPriceSpecification(50);
        var expression = specification.ToExpression();

        //Act
        var func = expression.Compile();
        var matchingProduct = new Product { Price = 50 };
        var higherProduct = new Product { Price = 100 };
        var lowerProduct = new Product { Price = 30 };

        //Assert
        Assert.True(func(matchingProduct));
        Assert.True(func(higherProduct));
        Assert.False(func(lowerProduct));
    }
}