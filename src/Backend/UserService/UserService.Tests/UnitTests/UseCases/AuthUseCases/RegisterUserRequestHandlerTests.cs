using System.Linq.Expressions;
using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.BLL.DTO;
using UserService.BLL.Exceptions;
using UserService.BLL.Proxy;
using UserService.BLL.UseCases.AuthUseCases.RegisterUserUseCase;
using UserService.DAL.Entities;
using UserService.DAL.Services.BlobStorage;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.MessageBrocker.ProducerService;
using UserService.DAL.Services.TemporaryStorage;
using UserService.Tests.Factories;

namespace UserService.Tests.UnitTests.UseCases.AuthUseCases;

public class RegisterUserRequestHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IBlobService> _blobServiceMock = new();
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<ICacheService> _cacheServiceMock = new();
    private readonly Mock<IProducerService> _producerServiceMock = new();
    private readonly Mock<ILogger<RegisterUserRequestHandler>> _loggerMock = new();
    private readonly Mock<IBackgroundJobProxy> _backgroundJobMock = new();
    private readonly RegisterUserRequestHandler _handler;
    private readonly Fixture _fixture = new();

    public RegisterUserRequestHandlerTests()
    {
        _userManagerMock = MocksFactory.CreateUserManager();
        _handler = new RegisterUserRequestHandler(
            _userManagerMock.Object,
            _mapperMock.Object,
            _blobServiceMock.Object,
            _emailServiceMock.Object,
            _cacheServiceMock.Object,
            _producerServiceMock.Object,
            _loggerMock.Object,
            _backgroundJobMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserRegistrationSucceeds_ShouldCreateUserAndQueueJobs()
    {
        //Arrange
        var avatarFileMock = new Mock<IFormFile>();

        var request = new RegisterUserRequest(new RegisterDTO { Avatar = avatarFileMock.Object });
        var user = _fixture.Create<AppUser>();
        
        var avatarStream = new Mock<Stream>();

        avatarFileMock.Setup(f => f.OpenReadStream()).Returns(avatarStream.Object);
        avatarFileMock.Setup(f => f.ContentType).Returns("image/png");
        
        _mapperMock.Setup(m => m.Map<AppUser>(request.RegisterModel)).Returns(user);
        _userManagerMock.Setup(manager => manager.CreateAsync(user, request.RegisterModel.Password))
            .ReturnsAsync(IdentityResult.Success);
        _blobServiceMock.Setup(service => service.UploadAsync(avatarStream.Object, "image/png"))
            .ReturnsAsync("avatarUrl");
        _cacheServiceMock.Setup(service => service.SetEmailConfirmationCodeAsync(user.Email))
            .ReturnsAsync("confirmationCode");
        
        //Act
        await _handler.Handle(request, CancellationToken.None);
        
        //Assert
        Assert.Equal("avatarUrl", user.AvatarUrl);
        _userManagerMock.Verify(manager => manager.CreateAsync(user, request.RegisterModel.Password), Times.Once);
        _backgroundJobMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Exactly(2));
        _backgroundJobMock.Verify(job => job.Schedule(It.IsAny<Expression<Func<Task>>>(), TimeSpan.FromMinutes(15)), Times.Once);
        _blobServiceMock.Verify(service => service.UploadAsync(avatarStream.Object, "image/png"), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserRegistrationSucceedsAndAvatarIsNull_ShouldCreateUserWithoutUploadingAvatar()
    {
        //Arrange
        var request = new RegisterUserRequest(new RegisterDTO { Avatar = null });
        var user = _fixture.Create<AppUser>();
        
        _mapperMock.Setup(m => m.Map<AppUser>(request.RegisterModel)).Returns(user);
        _userManagerMock.Setup(manager => manager.CreateAsync(user, request.RegisterModel.Password))
            .ReturnsAsync(IdentityResult.Success);
        _cacheServiceMock.Setup(service => service.SetEmailConfirmationCodeAsync(user.Email))
            .ReturnsAsync("confirmationCode");
        
        //Act
        await _handler.Handle(request, CancellationToken.None);
        
        //Assert
        _userManagerMock.Verify(manager => manager.CreateAsync(user, request.RegisterModel.Password), Times.Once);
        _backgroundJobMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Exactly(2));
        _backgroundJobMock.Verify(job => job.Schedule(It.IsAny<Expression<Func<Task>>>(), TimeSpan.FromMinutes(15)), Times.Once);
        _blobServiceMock.Verify(service => service.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_WhenUserRegistrationFails_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = new RegisterUserRequest(new RegisterDTO { Avatar = null });
        var user = _fixture.Create<AppUser>();

        _mapperMock.Setup(m => m.Map<AppUser>(request.RegisterModel)).Returns(user);
        _userManagerMock.Setup(manager => manager.CreateAsync(user, request.RegisterModel.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error message" }));

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Contains("Cannot register user: ", exception.Message);
        _userManagerMock.Verify(manager => manager.CreateAsync(user, request.RegisterModel.Password), Times.Once);
        _backgroundJobMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Never);
        _backgroundJobMock.Verify(job => job.Schedule(It.IsAny<Expression<Func<Task>>>(), It.IsAny<TimeSpan>()), Times.Never);
    }
    
    [Fact]
    public async Task DeleteUnconfirmedUser_WhenUserIsUnconfirmed_ShouldDeleteUserAndNotify()
    {
        //Arrange
        var email = "test@example.com";
        var user = _fixture.Create<AppUser>();
        user.EmailConfirmed = false;
        user.AvatarUrl = "avatarUrl";

        _userManagerMock.Setup(manager => manager.FindByEmailAsync(email))
            .ReturnsAsync(user);
        _blobServiceMock.Setup(service => service.DeleteAsync(user.AvatarUrl))
            .Returns(Task.CompletedTask);

        //Act
        await _handler.DeleteUnconfirmedUser(email);

        //Assert
        _userManagerMock.Verify(manager => manager.DeleteAsync(user), Times.Once);
        _blobServiceMock.Verify(service => service.DeleteAsync(user.AvatarUrl), Times.Once);
        _emailServiceMock.Verify(service => service.SendUnconfirmedAccountDeletedNotificationAsync(email), Times.Once);
    }
    
    [Fact]
    public async Task DeleteUnconfirmedUser_WhenUserIsUnconfirmedAndHasNoAvatar_ShouldDeleteUserWithoutDeletingAvatarAndNotify()
    {
        //Arrange
        var email = "test@example.com";
        var user = _fixture.Create<AppUser>();
        user.EmailConfirmed = false;
        user.AvatarUrl = null;

        _userManagerMock.Setup(manager => manager.FindByEmailAsync(email))
            .ReturnsAsync(user);

        //Act
        await _handler.DeleteUnconfirmedUser(email);

        //Assert
        _userManagerMock.Verify(manager => manager.DeleteAsync(user), Times.Once);
        _blobServiceMock.Verify(service => service.DeleteAsync(It.IsAny<string>()), Times.Never);
        _emailServiceMock.Verify(service => service.SendUnconfirmedAccountDeletedNotificationAsync(email), Times.Once);
    }
    
    [Fact]
    public async Task DeleteUnconfirmedUser_WhenUserIsConfirmed_ShouldNotDeleteUser()
    {
        //Arrange
        var email = "test@example.com";
        var user = _fixture.Create<AppUser>();
        user.EmailConfirmed = true;

        _userManagerMock.Setup(manager => manager.FindByEmailAsync(email))
            .ReturnsAsync(user);

        //Act
        await _handler.DeleteUnconfirmedUser(email);

        //Assert
        _userManagerMock.Verify(manager => manager.DeleteAsync(It.IsAny<AppUser>()), Times.Never);
        _blobServiceMock.Verify(service => service.DeleteAsync(It.IsAny<string>()), Times.Never);
        _emailServiceMock.Verify(service => service.SendUnconfirmedAccountDeletedNotificationAsync(It.IsAny<string>()), Times.Never);
    }
}
