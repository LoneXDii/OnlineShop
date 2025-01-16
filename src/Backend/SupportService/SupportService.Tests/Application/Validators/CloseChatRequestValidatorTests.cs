using SupportService.Application.UseCases.CloseChat;
using SupportService.Application.Validators;

namespace SupportService.Tests.Application.Validators;

public class CloseChatRequestValidatorTests
{
    private readonly CloseChatRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenEmptyUserIdAndChatIdIsGreaterThanZero_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Assert
        var request = new CloseChatRequest(1, "");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong user Id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenChatIdIsZeroAndUserIdIsNotEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Assert
        var request = new CloseChatRequest(0, "1");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong chat Id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenChatIdIsLessThanZeroAndUserIdIsNotEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Assert
        var request = new CloseChatRequest(-1, "1");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong chat Id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenChatIdIsZeroAndAndEmptyUserId_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Assert
        var request = new CloseChatRequest(0, "");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal("Wrong user Id", result.Errors[0].ErrorMessage);
        Assert.Equal("Wrong chat Id", result.Errors[1].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenChatIdIsLessThanZeroAndEmptyUserId_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Assert
        var request = new CloseChatRequest(-1, "");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal("Wrong user Id", result.Errors[0].ErrorMessage);
        Assert.Equal("Wrong chat Id", result.Errors[1].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenChatIdIsZeroAndUserIdIsNull_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Assert
        var request = new CloseChatRequest(0, null);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong chat Id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenChatIdIsLessThanZeroAndUserIdIsNull_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Assert
        var request = new CloseChatRequest(-1, null);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong chat Id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenChatIdIsGreaterThanZeroAndUserIdIsNotEmpty_ShouldReturnTrueWithEmptyErrorsList()
    {
        //Assert
        var request = new CloseChatRequest(1, "1");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WhenChatIdIsGreaterThanZeroAndUserIdIsNull_ShouldReturnTrueWithEmptyErrorsList()
    {
        //Assert
        var request = new CloseChatRequest(1, null);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
