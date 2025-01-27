using ProductsService.Application.UseCases.CategoryUseCases.Commands.DeleteCategory;
using ProductsService.Application.Validators.Categories;

namespace ProductsService.Tests.UnitTests.Validators.Categories;

public class DeleteCategoryRequestValidatorTests
{
    private readonly DeleteCategoryRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenCategoryIdIsZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new DeleteCategoryRequest(0);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong category id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenCategoryIdIsNegative_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new DeleteCategoryRequest(-1);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong category id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenCategoryIdIsValid_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var request = new DeleteCategoryRequest(1);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}