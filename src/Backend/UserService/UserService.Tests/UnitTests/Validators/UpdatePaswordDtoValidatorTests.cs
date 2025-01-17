using UserService.BLL.DTO;
using UserService.BLL.Validators;

namespace UserService.Tests.UnitTests.Validators;

public class UpdatePasswordDTOValidatorTests
{
    private readonly UpdatePasswordDTOValidator _validator = new();

    [Fact]
    public void Validate_WhenNewPasswordIsEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        // Arrange
        var changePasswordDto = new UpdatePasswordDTO { NewPassword = "" };

        // Act
        var result = _validator.Validate(changePasswordDto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal("Password must contains lower and uppercase letters, at least 1 digit and special symbol and be at least 8 symbols long", result.Errors[1].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenNewPasswordDoesNotMeetCriteria_ShouldReturnFalseWithCorrectErrorMessage()
    {
        // Arrange
        var changePasswordDto = new UpdatePasswordDTO { NewPassword = "weakpassword" };

        // Act
        var result = _validator.Validate(changePasswordDto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Password must contains lower and uppercase letters, at least 1 digit and special symbol and be at least 8 symbols long", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenNewPasswordIsValid_ShouldReturnTrueWithNoErrors()
    {
        // Arrange
        var changePasswordDto = new UpdatePasswordDTO { NewPassword = "Valid1Password!" };

        // Act
        var result = _validator.Validate(changePasswordDto);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
