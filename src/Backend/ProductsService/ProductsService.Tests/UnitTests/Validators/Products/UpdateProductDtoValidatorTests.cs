using Microsoft.AspNetCore.Http;
using Moq;
using ProductsService.Application.DTO;
using ProductsService.Application.Validators.Products;

namespace ProductsService.Tests.UnitTests.Validators.Products;

public class UpdateProductDtoValidatorTests
{
    private readonly UpdateProductDtoValidator _validator = new();

    [Fact]
    public void Validate_WhenNameIsEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new UpdateProductDTO { Name = string.Empty, Price = 10, Quantity = 1, Attributes = [] };

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong product name", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenPriceIsEqualToZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new UpdateProductDTO { Name = "Valid Product", Price = 0, Quantity = 1, Attributes = [] };

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong price", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenPriceIsLessThanZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new UpdateProductDTO { Name = "Valid Product", Price = -1, Quantity = 1, Attributes = [] };

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong price", result.Errors[0].ErrorMessage);
    }
    
    [Fact]
    public void Validate_WhenQuantityIsNegative_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new UpdateProductDTO { Name = "Valid Product", Price = 10, Quantity = -1, Attributes = [] };

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong quantity", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenImageIsNotAnImageFile_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.ContentType).Returns("application/pdf"); 
        var request = new UpdateProductDTO { Name = "Valid Product", Price = 10, Quantity = 1, Image = fileMock.Object, Attributes = [] };

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("You should upload only image files", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenImageIsAnImageFile_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.ContentType).Returns("image/png"); 
        var request = new UpdateProductDTO { Name = "Valid Product", Price = 10, Quantity = 1, Image = fileMock.Object, Attributes = [] };

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
    
    [Fact]
    public void Validate_WhenImageIsNull_ShouldNotReturnError()
    {
        //Arrange
        var request = new UpdateProductDTO { Name = "Valid Product", Price = 10, Quantity = 1, Image = null, Attributes = [] };

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WhenAttributesContainInvalidIds_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new UpdateProductDTO { Name = "Valid Product", Price = 10, Quantity = 1, Attributes = [1, 0, -2] };

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong param id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenAttributesAreEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new UpdateProductDTO { Name = "Valid Product", Price = 10, Quantity = 1, Attributes = [] };

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WhenAllPropertiesAreValid_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var request = new UpdateProductDTO
        {
            Name = "Valid Product",
            Price = 10,
            Quantity = 1,
            Image = null,
            Attributes = [1, 2, 3]
        };

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WhenAttributesAreNull_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var request = new UpdateProductDTO { Name = "Valid Product", Price = 10, Quantity = 1, Attributes = null };

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}