using ProductsService.Application.Specifications.Products;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UnitTests.Specifications.Products;

public class ProductMaxPriceSpecificationTests
{
    [Fact]
    public void IsSatisfiedBy_WhenMaxPriceIsNull_ShouldReturnTrue()
    {
        //Arrange
        var specification = new ProductMaxPriceSpecification(null);
        var product = new Product { Price = 100 };

        //Act
        var result = specification.IsSatisfiedBy(product);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_WhenProductPriceIsLessThanMaxPrice_ShouldReturnTrue()
    {
        //Arrange
        var specification = new ProductMaxPriceSpecification(100);
        var product = new Product { Price = 99 };

        //Act
        var result = specification.IsSatisfiedBy(product);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_WhenProductPriceIsEqualToMaxPrice_ShouldReturnTrue()
    {
        //Arrange
        var specification = new ProductMaxPriceSpecification(100);
        var product = new Product { Price = 100 };

        //Act
        var result = specification.IsSatisfiedBy(product);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_WhenProductPriceIsGreaterThanMaxPrice_ShouldReturnFalse()
    {
        //Arrange
        var specification = new ProductMaxPriceSpecification(50);
        var product = new Product { Price = 100 };

        //Act
        var result = specification.IsSatisfiedBy(product);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void ToExpression_WhenCalled_ShouldReturnCorrectExpression()
    {
        //Arrange
        var specification = new ProductMaxPriceSpecification(50);
        var expression = specification.ToExpression();

        //Act
        var func = expression.Compile();
        var matchingProduct = new Product { Price = 50 };
        var lowerProduct = new Product { Price = 30 };
        var nonMatchingProduct = new Product { Price = 100 };

        //Assert
        Assert.True(func(lowerProduct));
        Assert.True(func(matchingProduct));
        Assert.False(func(nonMatchingProduct));
    }
}