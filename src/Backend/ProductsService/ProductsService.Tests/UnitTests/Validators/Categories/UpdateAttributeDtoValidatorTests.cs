using ProductsService.Application.DTO;
using ProductsService.Application.Validators.Categories;

namespace ProductsService.Tests.UnitTests.Validators.Categories;

public class UpdateAttributeDtoValidatorTests
{
    private readonly UpdateAttributeDtoValidator _validator = new();

    [Fact]
    public void Validate_WhenAttributeIdIsZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new UpdateAttributeDTO { AttributeId = 0 };

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
        var request = new UpdateAttributeDTO { AttributeId = -1 };

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong attribute id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenAttributeIdIsValid_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var request = new UpdateAttributeDTO { AttributeId = 1 };

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}