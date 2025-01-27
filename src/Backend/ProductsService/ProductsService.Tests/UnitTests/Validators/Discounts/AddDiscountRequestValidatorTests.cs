using ProductsService.Application.UseCases.DiscountUseCases.Commands.AddDiscount;
using ProductsService.Application.Validators.Discounts;

namespace ProductsService.Tests.UnitTests.Validators.Discounts;

public class AddDiscountRequestValidatorTests
{
    private readonly AddDiscountRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenProductIdIsLessThanZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AddDiscountRequest(-10, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(1), 50);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong discount id", result.Errors[0].ErrorMessage);
    }
    
    [Fact]
    public void Validate_WhenProductIdIsZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AddDiscountRequest(0, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(1), 50);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong discount id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenPercentIsLessThanZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AddDiscountRequest(1, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(1), -50);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("'Percent' должно быть больше '0'.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenPercentIsGreaterThan100_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AddDiscountRequest(1, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(1), 150);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong discount percent", result.Errors[0].ErrorMessage);
    }
    
    [Fact]
    public void Validate_WhenPercentIsEqualToZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AddDiscountRequest(1, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(1), 0);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("'Percent' должно быть больше '0'.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenPercentIsEqualTo100_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AddDiscountRequest(1, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(1), 100);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong discount percent", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenStartDateIsInThePast_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AddDiscountRequest(1, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1), 50);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong discount start time", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenEndDateIsNotGreaterThanStartDate_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var date = DateTime.UtcNow.AddDays(1);
        var request = new AddDiscountRequest(1, date, date, 50);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong discount end time", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenAllPropertiesAreValid_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var request = new AddDiscountRequest(1, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 50);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}