using System.Linq.Expressions;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.BLL.Exceptions;
using UserService.BLL.Proxy;
using UserService.BLL.UseCases.UserUseCases.AskForResetPasswordUseCase;
using UserService.DAL.Entities;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;
using UserService.Tests.Factories;
using UserService.Tests.Setups;

namespace UserService.Tests.UnitTests.UseCases.UserUseCases;

public class AskForResetPasswordRequestHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<ICacheService> _cacheServiceMock = new();
    private readonly Mock<ILogger<AskForResetPasswordRequestHandler>> _loggerMock = new();
    private readonly Mock<IBackgroundJobProxy> _backgroundJobProxyMock = new();
    private readonly AskForResetPasswordRequestHandler _handler;
    private readonly Fixture _fixture = new();

    public AskForResetPasswordRequestHandlerTests()
    {
        _userManagerMock = MocksFactory.CreateUserManager();
        _backgroundJobProxyMock.SetupBackgroundJobProxy();
        
        _handler = new AskForResetPasswordRequestHandler(
            _userManagerMock.Object,
            _emailServiceMock.Object,
            _cacheServiceMock.Object,
            _loggerMock.Object,
            _backgroundJobProxyMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<AskForResetPasswordRequest>();
        _userManagerMock.Setup(manager => manager.FindByEmailAsync(request.Email))
            .ReturnsAsync((AppUser?)null);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such user", exception.Message);
        _backgroundJobProxyMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Never);
        _emailServiceMock.Verify(service => service.SendPasswordResetCodeAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserExists_ShouldSendPasswordResetCode()
    {
        //Arrange
        var request = _fixture.Create<AskForResetPasswordRequest>();
        var user = _fixture.Create<AppUser>();

        _userManagerMock.Setup(manager => manager.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);
        _cacheServiceMock.Setup(cache => cache.SetResetPasswordCodeAsync(request.Email))
            .ReturnsAsync("resetCode");

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _cacheServiceMock.Verify(cache => cache.SetResetPasswordCodeAsync(request.Email), Times.Once);
        _backgroundJobProxyMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Once);
        _emailServiceMock.Verify(service => service.SendPasswordResetCodeAsync(request.Email, "resetCode"), Times.Once);
    }
}