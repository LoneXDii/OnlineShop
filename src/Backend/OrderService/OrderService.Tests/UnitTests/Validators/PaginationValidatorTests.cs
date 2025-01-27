using OrderService.Application.DTO;
using OrderService.Application.Validators;

namespace OrderService.Tests.UnitTests.Validators;

public class PaginationValidatorTests
{
    private readonly PaginationValidator _validator = new();

    [Fact]
    public void Validate_WhenPageNoIsZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var pagination = new PaginationDTO { PageNo = 0, PageSize = 10 };

        //Act
        var result = _validator.Validate(pagination);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong page number", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenPageNoIsNegative_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var pagination = new PaginationDTO { PageNo = -1, PageSize = 10 };

        //Act
        var result = _validator.Validate(pagination);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong page number", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenPageSizeIsZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var pagination = new PaginationDTO { PageNo = 1, PageSize = 0 };

        //Act
        var result = _validator.Validate(pagination);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong page size", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenPageSizeIsNegative_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var pagination = new PaginationDTO { PageNo = 1, PageSize = -10 };

        //Act
        var result = _validator.Validate(pagination);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong page size", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenPageNoAndPageSizeAreValid_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var pagination = new PaginationDTO { PageNo = 1, PageSize = 10 };

        //Act
        var result = _validator.Validate(pagination);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
