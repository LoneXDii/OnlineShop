using ProductsService.Application.Specifications.Products;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UnitTests.Specifications.Products;

public class ProductsListByIdsSpecificationTests
{
    [Fact]
    public void IsSatisfiedBy_WhenProductIdIsInIds_ShouldReturnTrue()
    {
        //Arrange
        var specification = new ProductsListBuIdsSpecification(new List<int> { 1, 2, 3 });
        var product = new Product { Id = 1 };

        //Act
        var result = specification.IsSatisfiedBy(product);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_WhenProductIdIsNotInIds_ShouldReturnFalse()
    {
        //Arrange
        var specification = new ProductsListBuIdsSpecification(new List<int> { 1, 2, 3 });
        var product = new Product { Id = 4 };

        //Act
        var result = specification.IsSatisfiedBy(product);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void ToExpression_WhenCalledWithEmptyIds_ShouldReturnFalseForAnyProduct()
    {
        //Arrange
        var specification = new ProductsListBuIdsSpecification(new List<int>());
        var expression = specification.ToExpression();

        //Act
        var func = expression.Compile();
        var product = new Product { Id = 1 };

        //Assert
        Assert.False(func(product));
    }

    [Fact]
    public void ToExpression_WhenCalled_ShouldReturnCorrectExpression()
    {
        //Arrange
        var specification = new ProductsListBuIdsSpecification(new List<int> { 1, 2, 3 });
        var expression = specification.ToExpression();

        //Act
        var func = expression.Compile();
        var matchingProduct = new Product { Id = 2 };
        var nonMatchingProduct = new Product { Id = 4 };

        //Assert
        Assert.True(func(matchingProduct));
        Assert.False(func(nonMatchingProduct));
    }
}