using SupportService.Application.UseCases.GetUserChats;
using SupportService.Application.Validators;

namespace SupportService.Tests.Application.Validators;

public class GetUserChatsRequestValidatorTests
{
    private readonly GetUserChatsRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenUserIdIsEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new GetUserChatsRequest("");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong user Id", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenUserIdNotEmpty_ShouldReturnTrueWithEmptyErrorMessagesList()
    {
        //Arrange
        var request = new GetUserChatsRequest("1");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
