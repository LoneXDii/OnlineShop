using ProductsService.Application.Specifications.Categories;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UnitTests.Specifications.Categories;

public class TopLevelCategorySpecificationTests
{
    private readonly TopLevelCategorySpecification _specification = new TopLevelCategorySpecification();

    [Fact]
    public void IsSatisfiedBy_WhenCategoryIsTopLevel_ShouldReturnTrue()
    {
        //Arrange
        var category = new Category { ParentId = null };

        //Act
        var result = _specification.IsSatisfiedBy(category);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_WhenCategoryIsNotTopLevel_ShouldReturnFalse()
    {
        //Arrange
        var category = new Category { ParentId = 1 };

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
        var topLevelCategory = new Category { ParentId = null };
        var nonTopLevelCategory = new Category { ParentId = 2 };

        //Assert
        Assert.True(func(topLevelCategory));
        Assert.False(func(nonTopLevelCategory));
    }
}