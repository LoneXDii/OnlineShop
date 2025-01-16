using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using SupportService.Application.RequestsPipleneBehavior;

namespace SupportService.Tests.UnitTests.ValidationBehavior;

public class ValidationBehaviorTests
{
    private readonly Mock<IValidator<TestRequest>> _validatorMock;
    private readonly Mock<ILogger<ValidationBehavior<TestRequest, TestResponse>>> _loggerMock;
    private readonly ValidationBehavior<TestRequest, TestResponse> _validationBehavior;

    public ValidationBehaviorTests()
    {
        _validatorMock = new Mock<IValidator<TestRequest>>();
        _loggerMock = new Mock<ILogger<ValidationBehavior<TestRequest, TestResponse>>>();
        _validationBehavior = new ValidationBehavior<TestRequest, TestResponse>(_validatorMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldCallNext()
    {
        //Arrange
        var request = new TestRequest();
        var response = new TestResponse();
        var nextMock = new Mock<RequestHandlerDelegate<TestResponse>>();
        nextMock.Setup(next => next()).ReturnsAsync(response);
        _validatorMock.Setup(validator => validator.Validate(request)).Returns(new ValidationResult());

        //Act
        var result = await _validationBehavior.Handle(request, nextMock.Object, It.IsAny<CancellationToken>());

        // Assert
        Assert.Equal(response, result);
        nextMock.Verify(next => next(), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ShouldLogErrorAndThrowValidationException()
    {
        // Arrange
        var request = new TestRequest();
        var validationErrors = new List<ValidationFailure> { new ValidationFailure("Field", "Error message") };
        var validationResult = new ValidationResult(validationErrors);
        _validatorMock.Setup(v => v.Validate(request)).Returns(validationResult);

        // Act
        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            _validationBehavior.Handle(request, () => Task.FromResult(new TestResponse()), It.IsAny<CancellationToken>()));

        //Assert
        Assert.NotNull(exception);
        Assert.NotEmpty(exception.Message);
    }

    private class TestRequest { }

    private class TestResponse { }
}
