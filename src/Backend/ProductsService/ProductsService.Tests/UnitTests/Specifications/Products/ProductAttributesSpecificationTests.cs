using ProductsService.Application.Specifications.Products;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UnitTests.Specifications.Products;

public class ProductAttributesSpecificationTests
{
    [Fact]
    public void IsSatisfiedBy_WhenAttributeIdsIsNull_ShouldReturnTrue()
    {
        //Arrange
        var specification = new ProductAttributesSpecification(null);
        var product = new Product();

        //Act
        var result = specification.IsSatisfiedBy(product);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_WhenAllCategoriesMatch_ShouldReturnTrue()
    {
        //Arrange
        var specification = new ProductAttributesSpecification([1, 2]);
        var product = new Product
        {
            Categories = new List<Category>
            {
                new Category { Id = 1 },
                new Category { Id = 2 }
            }
        };

        //Act
        var result = specification.IsSatisfiedBy(product);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_WhenNotAllCategoriesMatch_ShouldReturnFalse()
    {
        //Arrange
        var specification = new ProductAttributesSpecification([1, 2]);
        var product = new Product
        {
            Categories = new List<Category>
            {
                new Category { Id = 1 }
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
        var specification = new ProductAttributesSpecification([1, 2]);
        var expression = specification.ToExpression();

        //Act
        var func = expression.Compile();
        
        var matchingProduct = new Product
        {
            Categories = new List<Category>
            {
                new Category { Id = 1 },
                new Category { Id = 2 }
            }
        };
        
        var nonMatchingProduct = new Product
        {
            Categories = new List<Category>
            {
                new Category { Id = 3 }
            }
        };

        //Assert
        Assert.True(func(matchingProduct));
        Assert.False(func(nonMatchingProduct));
    }
}