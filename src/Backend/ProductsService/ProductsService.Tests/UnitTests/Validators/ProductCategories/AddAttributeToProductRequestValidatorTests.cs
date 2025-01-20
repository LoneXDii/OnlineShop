using ProductsService.Application.UseCases.ProductUseCases.Commands.AddAttributeToProduct;
using ProductsService.Application.Validators.ProductCategories;

namespace ProductsService.Tests.UnitTests.Validators.ProductCategories;

public class AddAttributeToProductRequestValidatorTests
{
    private readonly AddAttributeToProductRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenProductIdIsZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AddAttributeToProductRequest(0, 1);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong product id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenProductIdIsNegative_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AddAttributeToProductRequest(-1, 1);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong product id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenAttributeIdIsZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AddAttributeToProductRequest(1, 0);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong attribute id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenAttributeIdIsNegative_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AddAttributeToProductRequest(1, -1);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong attribute id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenBothIdsAreValid_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var request = new AddAttributeToProductRequest(1, 1);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}