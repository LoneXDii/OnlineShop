using System.Linq.Expressions;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.BLL.Exceptions;
using UserService.BLL.Proxy;
using UserService.BLL.UseCases.UserUseCases.ResetPasswordUseCase;
using UserService.DAL.Entities;
using UserService.DAL.Services.Authentication;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;
using UserService.Tests.Factories;
using UserService.Tests.Setups;

namespace UserService.Tests.UnitTests.UseCases.UserUseCases;

public class ResetPasswordRequestHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<ICacheService> _cacheServiceMock = new();
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<ILogger<ResetPasswordRequestHandler>> _loggerMock = new();
    private readonly Mock<IBackgroundJobProxy> _backgroundJobMock = new();
    private readonly ResetPasswordRequestHandler _handler;
    private readonly Fixture _fixture;

    public ResetPasswordRequestHandlerTests()
    {
        _userManagerMock = MocksFactory.CreateUserManager();
        _backgroundJobMock.SetupBackgroundJobProxy();
        
        _handler = new ResetPasswordRequestHandler(
            _userManagerMock.Object,
            _cacheServiceMock.Object,
            _emailServiceMock.Object,
            _tokenServiceMock.Object,
            _loggerMock.Object,
            _backgroundJobMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_WhenResetPasswordCodeIsInvalid_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<ResetPasswordRequest>();
        _cacheServiceMock.Setup(cache => cache.GetEmailByResetPasswordCodeAsync(request.Code))
            .ReturnsAsync((string?)null);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such code", exception.Message);
        _userManagerMock.Verify(manager => manager.RemovePasswordAsync(It.IsAny<AppUser>()), Times.Never);
        _userManagerMock.Verify(manager => manager.AddPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Never);
        _backgroundJobMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Never);
        _tokenServiceMock.Verify(tokenService => tokenService.InvalidateRefreshTokenAsync(It.IsAny<AppUser>()), Times.Never);
        _emailServiceMock.Verify(service => service.SendPasswordResetSucceededNotificationAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<ResetPasswordRequest>();
        var email = _fixture.Create<string>();
        _cacheServiceMock.Setup(cache => cache.GetEmailByResetPasswordCodeAsync(request.Code))
            .ReturnsAsync(email);
        _userManagerMock.Setup(manager => manager.FindByEmailAsync(email))
            .ReturnsAsync((AppUser?)null);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such user", exception.Message);
        _userManagerMock.Verify(manager => manager.RemovePasswordAsync(It.IsAny<AppUser>()), Times.Never);
        _userManagerMock.Verify(manager => manager.AddPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Never);
        _backgroundJobMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Never);
        _tokenServiceMock.Verify(tokenService => tokenService.InvalidateRefreshTokenAsync(It.IsAny<AppUser>()), Times.Never);
        _emailServiceMock.Verify(service => service.SendPasswordResetSucceededNotificationAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenResettingPassword_ShouldResetPasswordAndSendNotification()
    {
        //Arrange
        var request = _fixture.Create<ResetPasswordRequest>();
        var email = _fixture.Create<string>();
        var user = _fixture.Create<AppUser>();

        _cacheServiceMock.Setup(cache => cache.GetEmailByResetPasswordCodeAsync(request.Code))
            .ReturnsAsync(email);
        _userManagerMock.Setup(manager => manager.FindByEmailAsync(email))
            .ReturnsAsync(user);
        _userManagerMock.Setup(manager => manager.RemovePasswordAsync(user))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(manager => manager.AddPasswordAsync(user, request.Password))
            .ReturnsAsync(IdentityResult.Success);
        _tokenServiceMock.Setup(tokenService => tokenService.InvalidateRefreshTokenAsync(user))
            .Returns(Task.CompletedTask);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _userManagerMock.Verify(manager => manager.RemovePasswordAsync(user), Times.Once);
        _userManagerMock.Verify(manager => manager.AddPasswordAsync(user, request.Password), Times.Once);
        _backgroundJobMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Once);
        _tokenServiceMock.Verify(tokenService => tokenService.InvalidateRefreshTokenAsync(user), Times.Once);
        _emailServiceMock.Verify(service => service.SendPasswordResetSucceededNotificationAsync(email), Times.Once);
    }
}