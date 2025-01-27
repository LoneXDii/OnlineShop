using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.API.Middleware;
using UserService.BLL.Exceptions;

namespace UserService.Tests.UnitTests.Middleware;

public class ExceptionMiddlewareTests
{
    private readonly Mock<RequestDelegate> _nextMock = new();
    private readonly Mock<ILogger<ExceptionMiddleware>> _loggerMock = new();
    private readonly Mock<IWebHostEnvironment> _envMock = new();
    private readonly ExceptionMiddleware _middleware;

    public ExceptionMiddlewareTests()
    {
        _middleware = new ExceptionMiddleware(_nextMock.Object, _loggerMock.Object, _envMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_WhenInvalidTokenExceptionThrown_ShouldReturnNotFound()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var exception = new InvalidTokenException("Invalid token");
        _nextMock.Setup(m => m(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        //Act
        await _middleware.InvokeAsync(context);

        //Assert
        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WhenValidationExceptionThrown_ShouldReturnBadRequest()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var exception = new ValidationException("Validation error");
        _nextMock.Setup(m => m(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        //Act
        await _middleware.InvokeAsync(context);

        //Assert
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldReturnNotFound()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var exception = new NotFoundException("Not found");
        _nextMock.Setup(m => m(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        //Act
        await _middleware.InvokeAsync(context);

        //Assert
        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WhenBadRequestExceptionThrown_ShouldReturnBadRequest()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var exception = new BadRequestException("Bad request");
        _nextMock.Setup(m => m(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        //Act
        await _middleware.InvokeAsync(context);

        //Assert
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WhenGeneralExceptionThrown_ShouldReturnInternalServerError()
    {
        //Arrange
        var context = new DefaultHttpContext();
        var exception = new Exception("General error");
        _nextMock.Setup(m => m(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        //Act
        await _middleware.InvokeAsync(context);

        //Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextMiddleware()
    {
        //Arrange
        var context = new DefaultHttpContext();
        _nextMock.Setup(m => m(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

        //Act
        await _middleware.InvokeAsync(context);

        //Assert
        _nextMock.Verify(m => m(context), Times.Once);
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
    }
}
