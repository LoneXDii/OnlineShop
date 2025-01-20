using ProductsService.Application.UseCases.DiscountUseCases.Commands.DeleteDiscount;
using ProductsService.Application.Validators.Discounts;

namespace ProductsService.Tests.UnitTests.Validators.Discounts;

public class DeleteDiscountRequestValidatorTests
{
    private readonly DeleteDiscountRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenDiscountIdIsZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new DeleteDiscountRequest(0);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong discount id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenDiscountIdIsNegative_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new DeleteDiscountRequest(-1);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong discount id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenDiscountIdIsValid_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var request = new DeleteDiscountRequest(1);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}