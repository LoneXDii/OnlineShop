using UserService.BLL.UseCases.AuthUseCases.ResendEmailConfirmationCodeUseCase;
using UserService.BLL.Validators;

namespace UserService.Tests.UnitTests.Validators;

public class ResendEmailConfirmationCodeRequestValidatorTests
{
    private readonly ResendEmailConfirmationCodeRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenEmailIsEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        // Arrange
        var request = new ResendEmailConfirmationCodeRequest("");

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal("Incorrect email adress", result.Errors[1].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenEmailIsInvalidFormat_ShouldReturnFalseWithCorrectErrorMessage()
    {
        // Arrange
        var request = new ResendEmailConfirmationCodeRequest("invalid-email");

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Incorrect email adress", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenEmailIsValid_ShouldReturnTrueWithNoErrors()
    {
        // Arrange
        var request = new ResendEmailConfirmationCodeRequest("test@example.com");

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
