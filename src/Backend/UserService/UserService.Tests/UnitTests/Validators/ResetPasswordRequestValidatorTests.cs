using UserService.BLL.UseCases.UserUseCases.ResetPasswordUseCase;
using UserService.BLL.Validators;

namespace UserService.Tests.UnitTests.Validators;

public class ResetPasswordRequestValidatorTests
{
    private readonly ResetPasswordRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenCodeIsEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new ResetPasswordRequest("Valid1Password!", "");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal("Wrong code", result.Errors[1].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenCodeIsNotSixCharacters_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new ResetPasswordRequest("Valid1Password!", "12345");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Wrong code", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenPasswordIsEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new ResetPasswordRequest("", "123456");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal("Password must contains lower and uppercase letters, at least 1 digit and special symbol and be at least 8 symbols long", result.Errors[1].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenPasswordDoesNotMeetCriteria_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var request = new ResetPasswordRequest("weakpassword", "123456");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Password must contains lower and uppercase letters, at least 1 digit and special symbol and be at least 8 symbols long", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenCodeAndPasswordAreValid_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var request = new ResetPasswordRequest("Valid1Password!", "123456");

        //Act
        var result = _validator.Validate(request);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
