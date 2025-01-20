using ProductsService.Application.Specifications.Categories;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UnitTests.Specifications.Categories;

public class ParentCategorySpecificationTests
{
    private readonly ParentCategorySpecification _specification= new ParentCategorySpecification(1);

    [Fact]
    public void IsSatisfiedBy_WhenCategoryParentIdMatches_ShouldReturnTrue()
    {
        //Arrange
        var category = new Category { ParentId = 1 };

        //Act
        var result = _specification.IsSatisfiedBy(category);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_WhenCategoryParentIdDoesNotMatch_ShouldReturnFalse()
    {
        //Arrange
        var category = new Category { ParentId = 2 };

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
        var categoryWithMatchingParentId = new Category { ParentId = 1 };
        var categoryWithNonMatchingParentId = new Category { ParentId = 3 };

        //Assert
        Assert.True(func(categoryWithMatchingParentId));
        Assert.False(func(categoryWithNonMatchingParentId));
    }
}