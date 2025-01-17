using UserService.BLL.UseCases.UserUseCases.AskForResetPasswordUseCase;
using UserService.BLL.Validators;

namespace UserService.Tests.UnitTests.Validators;

public class AskForResetPasswordRequestValidatorTests
{
    private readonly AskForResetPasswordRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenEmailIsEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AskForResetPasswordRequest(string.Empty);

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal("Incorrect email adress", result.Errors[1].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenEmailIsInvalidFormat_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new AskForResetPasswordRequest("invalid-email");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Incorrect email adress", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenEmailIsValid_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var request = new AskForResetPasswordRequest("user@example.com");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
