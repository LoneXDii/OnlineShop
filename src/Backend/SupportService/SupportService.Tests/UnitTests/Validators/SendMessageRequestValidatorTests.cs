using SupportService.Application.DTO;
using SupportService.Application.UseCases.SendMessage;
using SupportService.Application.Validators;

namespace SupportService.Tests.UnitTests.Validators;

public class SendMessageRequestValidatorTests
{
    private readonly SendMessageRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenMessageTextIsEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        // Assert
        var request = new SendMessageRequest(new AddMessageDTO { Text = "", ChatId = 1 }, "1");

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("You should enter message text", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenChatIdIsZeroAndUserIdIsNotEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        // Assert
        var request = new SendMessageRequest(new AddMessageDTO { Text = "Hello", ChatId = 0 }, "1");

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong chat id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenChatIdIsLessThanZeroAndUserIdIsNotEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        // Assert
        var request = new SendMessageRequest(new AddMessageDTO { Text = "Hello", ChatId = -1 }, "1");

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong chat id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenMessageTextIsEmptyAndChatIdIsZero_ShouldReturnFalseWithCorrectErrorMessages()
    {
        // Assert
        var request = new SendMessageRequest(new AddMessageDTO { Text = "", ChatId = 0 }, "");

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(3, result.Errors.Count);
        Assert.Equal("You should enter message text", result.Errors[0].ErrorMessage);
        Assert.Equal("Wrong chat id", result.Errors[1].ErrorMessage);
        Assert.Equal("Wrong user id", result.Errors[2].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenChatIdIsLessThanZeroAndMessageTextIsEmpty_ShouldReturnFalseWithCorrectErrorMessages()
    {
        // Assert
        var request = new SendMessageRequest(new AddMessageDTO { Text = "", ChatId = -1 }, "");

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(3, result.Errors.Count);
        Assert.Equal("You should enter message text", result.Errors[0].ErrorMessage);
        Assert.Equal("Wrong chat id", result.Errors[1].ErrorMessage);
        Assert.Equal("Wrong user id", result.Errors[2].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenChatIdIsGreaterThanZeroAndUserIdIsNotEmptyAndMessageTextIsNotEmpty_ShouldReturnTrueWithEmptyErrorsList()
    {
        // Assert
        var request = new SendMessageRequest(new AddMessageDTO { Text = "Hello", ChatId = 1 }, "1");

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
