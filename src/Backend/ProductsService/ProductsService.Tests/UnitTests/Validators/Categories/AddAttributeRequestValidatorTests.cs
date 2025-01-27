using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddAttribute;
using ProductsService.Application.Validators.Categories;

namespace ProductsService.Tests.UnitTests.Validators.Categories;

public class AddAttributeRequestValidatorTests
{
    private readonly AddAttributeRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenNameIsEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AddAttributeRequest(1, "");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong attribute name", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenParentIdIsZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AddAttributeRequest(0, "Attribute");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong parent id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenParentIdIsNegative_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AddAttributeRequest(-1, "Attribute");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong parent id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenNameIsNotEmptyAndParentIdIsValid_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var request = new AddAttributeRequest(1, "Attribute");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}