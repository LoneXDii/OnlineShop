using System.Linq.Expressions;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.BLL.Exceptions;
using UserService.BLL.Proxy;
using UserService.BLL.UseCases.AuthUseCases.EmailConfirmationUseCase;
using UserService.DAL.Entities;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;
using UserService.Tests.Factories;
using UserService.Tests.Setups;

namespace UserService.Tests.UnitTests.UseCases.AuthUseCases;

public class EmailConfirmationRequestHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<ICacheService> _cacheServiceMock = new();
    private readonly Mock<ILogger<EmailConfirmationRequestHandler>> _loggerMock = new();
    private readonly Mock<IBackgroundJobProxy> _backgroundJobProxyMock = new();
    private readonly EmailConfirmationRequestHandler _handler;
    private readonly Fixture _fixture = new();
    
    public EmailConfirmationRequestHandlerTests()
    {
        _userManagerMock = MocksFactory.CreateUserManager();
        
        _backgroundJobProxyMock.SetupBackgroundJobProxy();
        
        _handler = new EmailConfirmationRequestHandler(
            _userManagerMock.Object, 
            _emailServiceMock.Object, 
            _cacheServiceMock.Object, 
            _loggerMock.Object,
            _backgroundJobProxyMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExists_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<EmailConfirmationRequest>();
        
        _userManagerMock.Setup(manager => manager.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((AppUser?)null);
        
        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        
        //Assert
        Assert.Equal("No user with such email", exception.Message);
        _userManagerMock.Verify(manager => manager.UpdateAsync(It.IsAny<AppUser>()), Times.Never);
        _backgroundJobProxyMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Never);
        _emailServiceMock.Verify(service => service.SendEmailConfirmationSucceededNotificationAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenConfirmationCodeIsWrong_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<EmailConfirmationRequest>();
        var user = _fixture.Create<AppUser>();
        
        _userManagerMock.Setup(manager => manager.FindByEmailAsync(request.email))
            .ReturnsAsync(user);
        
        _cacheServiceMock.Setup(service => service.GetEmailByCodeAsync(request.code))
            .ReturnsAsync($"{user.Email}123"); 
        
        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
        
        //Assert
        Assert.Equal("Wrong code", exception.Message);
        _userManagerMock.Verify(manager => manager.UpdateAsync(It.IsAny<AppUser>()), Times.Never);
        _backgroundJobProxyMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Never);
        _emailServiceMock.Verify(service => service.SendEmailConfirmationSucceededNotificationAsync(It.IsAny<string>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_WhenEmailConfirmed_ShouldUpdateUserAndSendNotification()
    {
        //Arrange
        var request = _fixture.Create<EmailConfirmationRequest>();
        var user = _fixture.Create<AppUser>();
        user.EmailConfirmed = false;
        
        _userManagerMock.Setup(manager => manager.FindByEmailAsync(request.email))
            .ReturnsAsync(user);
        
        _cacheServiceMock.Setup(service => service.GetEmailByCodeAsync(request.code))
            .ReturnsAsync(user.Email);
        
        //Act
        await _handler.Handle(request, CancellationToken.None);
        
        //Assert
        Assert.True(user.EmailConfirmed);
        _userManagerMock.Verify(manager => manager.UpdateAsync(user), Times.Once);
        _backgroundJobProxyMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Once);
        _emailServiceMock.Verify(service => service.SendEmailConfirmationSucceededNotificationAsync(user.Email), Times.Once);
    }
}