using UserService.BLL.DTO;
using UserService.BLL.Validators;

namespace UserService.Tests.UnitTests.Validators;

public class RegisterDTOValidatorTests
{
    private readonly RegisterDTOValidator _validator = new();

    [Fact]
    public void Validate_WhenEmailIsEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var user = new RegisterDTO { Email = "", Password = "Valid1Password!" };

        //Act
        var result = _validator.Validate(user);

        //Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal("Incorrect email adress", result.Errors[1].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenEmailIsInvalid_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var user = new RegisterDTO { Email = "invalid-email", Password = "Valid1Password!" };

        //Act
        var result = _validator.Validate(user);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Incorrect email adress", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenPasswordIsEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var user = new RegisterDTO { Email = "test@example.com", Password = "" };

        //Act
        var result = _validator.Validate(user);

        //Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal("Password must contains lower and uppercase letters, at least 1 digit and special symbol and be at least 8 symbols long", result.Errors[1].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenPasswordDoesNotMeetCriteria_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var user = new RegisterDTO { Email = "test@example.com", Password = "weakpassword" };

        //Act
        var result = _validator.Validate(user);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Password must contains lower and uppercase letters, at least 1 digit and special symbol and be at least 8 symbols long", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenEmailAndPasswordAreValid_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var user = new RegisterDTO { Email = "test@example.com", Password = "Valid1Password!" };

        //Act
        var result = _validator.Validate(user);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
