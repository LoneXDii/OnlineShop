using ProductsService.Application.UseCases.ProductUseCases.Queries.ListProducts;
using ProductsService.Application.Validators.Products;

namespace ProductsService.Tests.UnitTests.Validators.Products;

public class ListProductsWithPaginationRequestValidatorTests
{
    private readonly ListProductsWithPaginationRequestValidator _validator = new();
    
    [Fact]
    public void Validate_WhenMinPriceIsEqualToZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        // Arrange
        var request = new ListProductsWithPaginationRequest(10, 0, 1, 1, 10, [1, 2, 3]);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong min price", result.Errors[0].ErrorMessage);
    }
    
    [Fact]
    public void Validate_WhenMinPriceIsLessThanZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        // Arrange
        var request = new ListProductsWithPaginationRequest(10, -1, 1, 1, 10, [1, 2, 3]);

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong min price", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenMaxPriceIsEqualToZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new ListProductsWithPaginationRequest(0, 1, 1, 1, 10, [1, 2, 3]);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal("Wrong max price", result.Errors[0].ErrorMessage);
        Assert.Equal("Max price cant be less than min price", result.Errors[1].ErrorMessage);
    }
    
    [Fact]
    public void Validate_WhenMaxPriceIsLessThanZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new ListProductsWithPaginationRequest(-10, 1, 1, 1, 10, [1, 2, 3]);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal("Wrong max price", result.Errors[0].ErrorMessage);
        Assert.Equal("Max price cant be less than min price", result.Errors[1].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenMaxPriceIsLessThanMinPrice_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new ListProductsWithPaginationRequest(1, 10, 1, 1, 10, [1, 2, 3]);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Max price cant be less than min price", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenCategoryIdIsEqualToZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new ListProductsWithPaginationRequest(10, 1, 0, 1, 10, [1, 2, 3]);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong category id", result.Errors[0].ErrorMessage);
    }
    
    [Fact]
    public void Validate_WhenCategoryIdIsLessThanZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new ListProductsWithPaginationRequest(10, 1, -1, 1, 10, [1, 2, 3]);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong category id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenValuesIdsContainInvalidIds_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new ListProductsWithPaginationRequest(10, 1, 1, 1, 10, [1, -2, 3]);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong values ids", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenAllPropertiesAreValid_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var request = new ListProductsWithPaginationRequest(10, 1, 1, 1, 10, [1, 2, 3]);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WhenValuesIdsIsNull_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var request = new ListProductsWithPaginationRequest(10, 1, 1, 1, 10, null);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}