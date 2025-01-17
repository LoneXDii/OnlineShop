using OrderService.Application.UseCases.CartUseCases.RemoveItemFromCartUseCase;
using OrderService.Application.Validators;

namespace OrderService.Tests.UnitTests.Validators;

public class RemoveItemFromCartRequestValidatorTests
{
    private readonly RemoveItemFromCartRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenItemIdIsZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new RemoveItemFromCartRequest(0);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong item id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenItemIdIsNegative_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new RemoveItemFromCartRequest(-1);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong item id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenItemIdIsPositive_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var request = new RemoveItemFromCartRequest(1);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
