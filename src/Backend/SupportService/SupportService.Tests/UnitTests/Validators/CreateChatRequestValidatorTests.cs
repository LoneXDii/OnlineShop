using SupportService.Application.UseCases.CreateChat;
using SupportService.Application.Validators;

namespace SupportService.Tests.UnitTests.Validators;

public class CreateChatRequestValidatorTests
{
    private readonly CreateChatRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenClientIdIsEmptyAndClientNameIsNotEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new CreateChatRequest("", "a");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong client id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenClientIdIsIsNotEmptyAndClientNameIsEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new CreateChatRequest("a", "");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong client name", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenClientIdIsAndClientNameAreEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new CreateChatRequest("", "");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal("Wrong client id", result.Errors[0].ErrorMessage);
        Assert.Equal("Wrong client name", result.Errors[1].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenClientIdIsAndClientNameAreNotEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new CreateChatRequest("1", "1");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
