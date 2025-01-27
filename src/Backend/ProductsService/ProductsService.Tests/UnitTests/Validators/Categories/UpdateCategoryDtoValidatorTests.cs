using Microsoft.AspNetCore.Http;
using Moq;
using ProductsService.Application.DTO;
using ProductsService.Application.Validators.Categories;

namespace ProductsService.Tests.UnitTests.Validators.Categories;

public class UpdateCategoryDtoValidatorTests
{
    private readonly UpdateCategoryDtoValidator _validator = new();
    private readonly Mock<IFormFile> _formFileMock = new();

    [Fact]
    public void Validate_WhenNameIsEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var categoryDto = new UpdateCategoryDTO { Name = string.Empty, Image = null };

        //Act
        var result = _validator.Validate(categoryDto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong category name", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenImageIsNotAnImageFile_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        _formFileMock.Setup(f => f.ContentType).Returns("application/pdf");
        var categoryDto = new UpdateCategoryDTO { Name = "Valid Name", Image = _formFileMock.Object };

        //Act
        var result = _validator.Validate(categoryDto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("You should upload only image files", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenImageIsNull_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var categoryDto = new UpdateCategoryDTO { Name = "Valid Name", Image = null };

        //Act
        var result = _validator.Validate(categoryDto);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WhenImageIsValidImageFile_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        _formFileMock.Setup(f => f.ContentType).Returns("image/png");
        var categoryDto = new UpdateCategoryDTO() { Name = "Valid Name", Image = _formFileMock.Object };

        //Act
        var result = _validator.Validate(categoryDto);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}