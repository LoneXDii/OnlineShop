using ProductsService.Application.Specifications.Products;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UnitTests.Specifications.Products;

public class ProductCategorySpecificationTests
{
    [Fact]
    public void IsSatisfiedBy_WhenCategoryIdIsNull_ShouldReturnTrue()
    {
        //Arrange
        var specification = new ProductCategorySpecification(null);
        var product = new Product();

        //Act
        var result = specification.IsSatisfiedBy(product);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_WhenCategoryExistsAndIsTopLevel_ShouldReturnTrue()
    {
        //Arrange
        var specification = new ProductCategorySpecification(1);
        var product = new Product
        {
            Categories = new List<Category>
            {
                new Category { Id = 1, ParentId = null },
                new Category { Id = 2, ParentId = 1 }
            }
        };

        //Act
        var result = specification.IsSatisfiedBy(product);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_WhenCategoryExistsButIsNotTopLevel_ShouldReturnFalse()
    {
        //Arrange
        var specification = new ProductCategorySpecification(1);
        var product = new Product
        {
            Categories = new List<Category>
            {
                new Category { Id = 2, ParentId = 1 }
            }
        };

        //Act
        var result = specification.IsSatisfiedBy(product);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void ToExpression_WhenCalled_ShouldReturnCorrectExpression()
    {
        //Arrange
        var specification = new ProductCategorySpecification(1);
        var expression = specification.ToExpression();

        //Act
        var func = expression.Compile();
        
        var matchingProduct = new Product
        {
            Categories = new List<Category>
            {
                new Category { Id = 1, ParentId = null }
            }
        };
        
        var nonMatchingProduct = new Product
        {
            Categories = new List<Category>
            {
                new Category { Id = 2, ParentId = 1 }
            }
        };

        //Assert
        Assert.True(func(matchingProduct));
        Assert.False(func(nonMatchingProduct));
    }
}