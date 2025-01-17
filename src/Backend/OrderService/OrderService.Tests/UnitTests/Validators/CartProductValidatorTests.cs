using OrderService.Application.DTO;
using OrderService.Application.Validators;

namespace OrderService.Tests.UnitTests.Validators;

public class CartProductValidatorTests
{
    private readonly CartProductValidator _validator = new();

    [Fact]
    public void Validate_WhenQuantityIsZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        // Arrange
        var product = new CartProductDTO { Quantity = 0, Id = 1 };

        // Act
        var result = _validator.Validate(product);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Incorrect quantity", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenQuantityIsNegative_ShouldReturnFalseWithCorrectErrorMessage()
    {
        // Arrange
        var product = new CartProductDTO { Quantity = -1, Id = 1 };

        // Act
        var result = _validator.Validate(product);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Incorrect quantity", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenProductIdIsZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        // Arrange
        var product = new CartProductDTO { Quantity = 1, Id = 0 };

        // Act
        var result = _validator.Validate(product);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Incorrect product id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenProductIdIsNegative_ShouldReturnFalseWithCorrectErrorMessage()
    {
        // Arrange
        var product = new CartProductDTO { Quantity = 1, Id = -1 };

        // Act
        var result = _validator.Validate(product);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Incorrect product id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenQuantityAndProductIdAreValid_ShouldReturnTrueWithNoErrors()
    {
        // Arrange
        var product = new CartProductDTO { Quantity = 1, Id = 1 };

        // Act
        var result = _validator.Validate(product);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
