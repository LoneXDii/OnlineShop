using System.Linq.Expressions;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.BLL.Exceptions;
using UserService.BLL.Proxy;
using UserService.BLL.UseCases.UserUseCases.UpdateEmailUseCase;
using UserService.DAL.Entities;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;
using UserService.Tests.Factories;
using UserService.Tests.Setups;

namespace UserService.Tests.UnitTests.UseCases.UserUseCases;

public class UpdateEmailRequestHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<ICacheService> _cacheServiceMock = new();
    private readonly Mock<ILogger<UpdateEmailRequestHandler>> _loggerMock = new();
    private readonly Mock<IBackgroundJobProxy> _backgroundJobMock = new();
    private readonly UpdateEmailRequestHandler _handler;
    private readonly Fixture _fixture;

    public UpdateEmailRequestHandlerTests()
    {
        _userManagerMock = MocksFactory.CreateUserManager();
        _backgroundJobMock.SetupBackgroundJobProxy();

        _handler = new UpdateEmailRequestHandler(
            _userManagerMock.Object,
            _emailServiceMock.Object,
            _cacheServiceMock.Object,
            _loggerMock.Object,
            _backgroundJobMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_WhenNewEmailIsAlreadyInUse_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<UpdateEmailRequest>();
        var existingUser = _fixture.Create<AppUser>();

        _userManagerMock.Setup(manager => manager.FindByEmailAsync(request.newEmail))
            .ReturnsAsync(existingUser);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("This email is already in use", exception.Message);
        _userManagerMock.Verify(manager => manager.FindByIdAsync(It.IsAny<string>()), Times.Never);
        _backgroundJobMock.Verify(job => job.Schedule(It.IsAny<Expression<Func<Task>>>(), It.IsAny<TimeSpan>()), Times.Never);
        _backgroundJobMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Never);
        _emailServiceMock.Verify(service => service.SendEmailConfirmationCodeAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<UpdateEmailRequest>();
        _userManagerMock.Setup(manager => manager.FindByEmailAsync(request.newEmail))
            .ReturnsAsync((AppUser?)null);
        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.userId))
            .ReturnsAsync((AppUser?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such user", exception.Message);
        _userManagerMock.Verify(manager => manager.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        _backgroundJobMock.Verify(job => job.Schedule(It.IsAny<Expression<Func<Task>>>(), It.IsAny<TimeSpan>()), Times.Never);
        _backgroundJobMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Never);
        _emailServiceMock.Verify(service => service.SendEmailConfirmationCodeAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenEmailIsUpdated_ShouldScheduleBackgroundJobAndSendConfirmationEmail()
    {
        //Arrange
        var request = _fixture.Create<UpdateEmailRequest>();
        var user = _fixture.Create<AppUser>();

        _userManagerMock.Setup(manager => manager.FindByEmailAsync(request.newEmail))
            .ReturnsAsync((AppUser?)null);
        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.userId))
            .ReturnsAsync(user);
        _userManagerMock.Setup(manager => manager.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);
        _cacheServiceMock.Setup(cache => cache.SetEmailConfirmationCodeAsync(request.newEmail))
            .ReturnsAsync("confirmation_code");

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal(request.newEmail, user.Email);
        Assert.Equal(request.newEmail, user.UserName);
        Assert.False(user.EmailConfirmed);
        _userManagerMock.Verify(manager => manager.UpdateAsync(user), Times.Once);
        _backgroundJobMock.Verify(job => job.Schedule(It.IsAny<Expression<Func<Task>>>(), It.IsAny<TimeSpan>()), Times.Once);
        _backgroundJobMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Once);
        _emailServiceMock.Verify(service => service.SendEmailConfirmationCodeAsync(user.Email, "confirmation_code"), Times.Once);
    }

    [Fact]
    public async Task ReturnOldEmailAsync_WhenEmailIsNotConfirmed_ShouldReturnOldEmailAndNotify()
    {
        //Arrange
        var oldEmail = _fixture.Create<string>();
        var newEmail = _fixture.Create<string>();
        var user = _fixture.Create<AppUser>();
        user.EmailConfirmed = false;
        
        _userManagerMock.Setup(manager => manager.FindByEmailAsync(newEmail))
            .ReturnsAsync(user);
        
        //Act
        await _handler.ReturnOldEmailAsync(oldEmail, newEmail);
        
        //Assert
        Assert.Equal(oldEmail, user.Email);
        _userManagerMock.Verify(manager => manager.UpdateAsync(user), Times.Once);
        _emailServiceMock.Verify(service => service.SendEmailNotChangedNotificationAsync(oldEmail, newEmail), Times.Once);
    }
    
    [Fact]
    public async Task ReturnOldEmailAsync_WhenUserIsNotFound_ShouldNotDoActions()
    {
        //Arrange
        var oldEmail = _fixture.Create<string>();
        var newEmail = _fixture.Create<string>();
        
        _userManagerMock.Setup(manager => manager.FindByEmailAsync(newEmail))
            .ReturnsAsync((AppUser?)null);
        
        //Act
        await _handler.ReturnOldEmailAsync(oldEmail, newEmail);
        
        //Assert
        _userManagerMock.Verify(manager => manager.UpdateAsync(It.IsAny<AppUser>()), Times.Never);
        _emailServiceMock.Verify(service => service.SendEmailNotChangedNotificationAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
    
    [Fact]
    public async Task ReturnOldEmailAsync_WhenNewEmailIsConfirmed_ShouldNotDoActions()
    {
        //Arrange
        var oldEmail = _fixture.Create<string>();
        var newEmail = _fixture.Create<string>();
        var user = _fixture.Create<AppUser>();
        user.EmailConfirmed = true;
        
        _userManagerMock.Setup(manager => manager.FindByEmailAsync(newEmail))
            .ReturnsAsync((AppUser?)null);
        
        //Act
        await _handler.ReturnOldEmailAsync(oldEmail, newEmail);
        
        //Assert
        _userManagerMock.Verify(manager => manager.UpdateAsync(It.IsAny<AppUser>()), Times.Never);
        _emailServiceMock.Verify(service => service.SendEmailNotChangedNotificationAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}