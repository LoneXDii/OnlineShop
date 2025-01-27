using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.BLL.Exceptions;
using UserService.BLL.UseCases.AuthUseCases.LogoutUserUseCase;
using UserService.DAL.Entities;
using UserService.DAL.Services.Authentication;
using UserService.Tests.Factories;

namespace UserService.Tests.UnitTests.UseCases.AuthUseCases;

public class LogoutUserRequestHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<ILogger<LogoutUserRequestHandler>> _loggerMock = new();
    private readonly LogoutUserRequestHandler _handler;
    private readonly Fixture _fixture = new();

    public LogoutUserRequestHandlerTests()
    {
        _userManagerMock = MocksFactory.CreateUserManager();
        
        _handler = new LogoutUserRequestHandler(
            _userManagerMock.Object,
            _tokenServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<LogoutUserRequest>();
        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.userId))
            .ReturnsAsync((AppUser?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such user", exception.Message);
        _tokenServiceMock.Verify(service => service.InvalidateRefreshTokenAsync(It.IsAny<AppUser>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserExists_ShouldInvalidateRefreshToken()
    {
        //Arrange
        var request = _fixture.Create<LogoutUserRequest>();
        var user = _fixture.Create<AppUser>();

        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.userId))
            .ReturnsAsync(user);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _tokenServiceMock.Verify(service => service.InvalidateRefreshTokenAsync(user), Times.Once);
    }
}