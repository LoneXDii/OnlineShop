using SupportService.Application.UseCases.GetChatMessages;
using SupportService.Application.Validators;

namespace SupportService.Tests.UnitTests.Validators;

public class GetChatMessagesRequestValidatorTests
{
    private readonly GetChatMessagesRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenChatIdIsZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new GetChatMessagesRequest(0);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong chat id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenChatIdIsLessThanZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new GetChatMessagesRequest(-1);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong chat id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenChatIdIsGreaterThanZero_ShouldReturnTrueWithEmptyErrorMessage()
    {
        //Arrange
        var request = new GetChatMessagesRequest(1);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
