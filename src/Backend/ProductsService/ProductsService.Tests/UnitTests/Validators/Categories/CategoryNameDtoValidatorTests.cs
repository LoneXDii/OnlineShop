using ProductsService.Application.DTO;
using ProductsService.Application.Validators.Categories;

namespace ProductsService.Tests.UnitTests.Validators.Categories;

public class CategoryNameDtoValidatorTests
{
    private readonly CategoryNameDtoValidator _validator = new();

    [Fact]
    public void Validate_WhenNameIsEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var categoryNameDto = new CategoryNameDTO { Name = string.Empty };

        //Act
        var result = _validator.Validate(categoryNameDto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong name", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenNameIsWhitespace_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var categoryNameDto = new CategoryNameDTO { Name = "   " }; 

        //Act
        var result = _validator.Validate(categoryNameDto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong name", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenNameIsValid_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var categoryNameDto = new CategoryNameDTO { Name = "Valid Name" };

        //Act
        var result = _validator.Validate(categoryNameDto);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}