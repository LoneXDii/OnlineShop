using OrderService.Application.DTO;
using OrderService.Application.Validators;

namespace OrderService.Tests.UnitTests.Validators;

public class QuantityDtoValidatorTests
{
    private readonly QuantityDtoValidator _validator = new();

    [Fact]
    public void Validate_WhenQuantityIsNegative_ShouldReturnFalseWithCorrectErrorMessage()
    {
        // Arrange
        var quantityDto = new QuantityDTO { Quantity = -1 };

        // Act
        var result = _validator.Validate(quantityDto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong quantity", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenQuantityIsZero_ShouldReturnTrueWithNoErrors()
    {
        // Arrange
        var quantityDto = new QuantityDTO { Quantity = 0 };

        // Act
        var result = _validator.Validate(quantityDto);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WhenQuantityIsPositive_ShouldReturnTrueWithNoErrors()
    {
        // Arrange
        var quantityDto = new QuantityDTO { Quantity = 10 };

        // Act
        var result = _validator.Validate(quantityDto);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
