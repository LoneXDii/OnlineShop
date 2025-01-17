using UserService.BLL.DTO;
using UserService.BLL.Validators;

namespace UserService.Tests.UnitTests.Validators;

public class UpdateEmailDTOValidatorTests
{
    private readonly UpdateEmailDTOValidator _validator = new();

    [Fact]
    public void Validate_WhenEmailIsEmpty_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var emailDto = new EmailDTO { Email = "" };

        //Act
        var result = _validator.Validate(emailDto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal("Incorrect email adress", result.Errors[1].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenEmailIsInvalidFormat_ShouldReturnFalseWithCorrectErrorMessage()
    {
        //Arrange
        var emailDto = new EmailDTO { Email = "invalid-email" };

        //Act
        var result = _validator.Validate(emailDto);

        //Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Incorrect email adress", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Validate_WhenEmailIsValid_ShouldReturnTrueWithNoErrors()
    {
        //Arrange
        var emailDto = new EmailDTO { Email = "test@example.com" };

        //Act
        var result = _validator.Validate(emailDto);

        //Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
