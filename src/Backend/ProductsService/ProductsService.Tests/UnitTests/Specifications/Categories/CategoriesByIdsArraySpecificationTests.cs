using ProductsService.Application.Specifications.Categories;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UnitTests.Specifications.Categories;

public class CategoriesByIdsArraySpecificationTests
{
    private readonly CategoriesByIdsArraySpecification _specification= new CategoriesByIdsArraySpecification([1, 2, 3]);

    [Fact]
    public void IsSatisfiedBy_WhenCategoryIdIsInIdsArray_ShouldReturnTrue()
    {
        //Arrange
        var category = new Category { Id = 2 };

        //Act
        var result = _specification.IsSatisfiedBy(category);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_WhenCategoryIdIsNotInIdsArray_ShouldReturnFalse()
    {
        //Arrange
        var category = new Category { Id = 4 };

        //Act
        var result = _specification.IsSatisfiedBy(category);

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
        var categoryInIds = new Category { Id = 1 };
        var categoryNotInIds = new Category { Id = 5 };

        //Assert
        Assert.True(func(categoryInIds));
        Assert.False(func(categoryNotInIds));
    }
}