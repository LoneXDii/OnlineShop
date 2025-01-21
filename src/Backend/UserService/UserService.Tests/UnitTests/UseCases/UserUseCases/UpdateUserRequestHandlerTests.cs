using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.BLL.DTO;
using UserService.BLL.Exceptions;
using UserService.BLL.UseCases.UserUseCases.UpdateUserUseCase;
using UserService.DAL.Entities;
using UserService.DAL.Services.BlobStorage;
using UserService.Tests.Factories;

namespace UserService.Tests.UnitTests.UseCases.UserUseCases;

public class UpdateUserRequestHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IBlobService> _blobServiceMock = new();
    private readonly Mock<ILogger<UpdateUserRequestHandler>> _loggerMock = new();
    private readonly UpdateUserRequestHandler _handler;
    private readonly Fixture _fixture;

    public UpdateUserRequestHandlerTests()
    {
        _userManagerMock = MocksFactory.CreateUserManager();
        _handler = new UpdateUserRequestHandler(
            _userManagerMock.Object,
            _mapperMock.Object,
            _blobServiceMock.Object,
            _loggerMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = new UpdateUserRequest(new UpdateUserDTO{Avatar = null}, "qwe");
        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.userId))
            .ReturnsAsync((AppUser?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such user", exception.Message);
        _userManagerMock.Verify(manager => manager.UpdateAsync(It.IsAny<AppUser>()), Times.Never);
        _blobServiceMock.Verify(service => service.DeleteAsync(It.IsAny<string>()), Times.Never);
        _blobServiceMock.Verify(service => service.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUpdatingUserWithoutAvatar_ShouldUpdateUserSuccessfully()
    {
        //Arrange
        var request = new UpdateUserRequest(new UpdateUserDTO{Avatar = null}, "qwe");
        var user = _fixture.Create<AppUser>();

        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.userId))
            .ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map(request.userDTO, user)).Verifiable();

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _userManagerMock.Verify(manager => manager.UpdateAsync(user), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map(request.userDTO, user), Times.Once);
        _blobServiceMock.Verify(service => service.DeleteAsync(It.IsAny<string>()), Times.Never);
        _blobServiceMock.Verify(service => service.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUpdatingUserWithAvatar_ShouldUploadNewAvatarAndDeleteOldAvatar()
    {
        //Arrange
        var avatarFile = new Mock<IFormFile>();
        var request = new UpdateUserRequest(new UpdateUserDTO{Avatar = avatarFile.Object}, "qwe");
        var user = _fixture.Create<AppUser>();
        
        var avatarStream = new Mock<Stream>();
        avatarFile.Setup(f => f.OpenReadStream()).Returns(avatarStream.Object);
        avatarFile.Setup(f => f.ContentType).Returns("image/png");
        request.userDTO.Avatar = avatarFile.Object;
        user.AvatarUrl = "old_avatar_url";

        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.userId))
            .ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map(request.userDTO, user)).Verifiable();
        _blobServiceMock.Setup(service => service.DeleteAsync(user.AvatarUrl)).Returns(Task.CompletedTask);
        _blobServiceMock.Setup(service => service.UploadAsync(avatarStream.Object, avatarFile.Object.ContentType))
            .ReturnsAsync("new_avatar_url");

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _blobServiceMock.Verify(service => service.DeleteAsync("old_avatar_url"), Times.Once);
        _blobServiceMock.Verify(service => service.UploadAsync(avatarStream.Object, avatarFile.Object.ContentType), Times.Once);
        Assert.Equal("new_avatar_url", user.AvatarUrl);
        _userManagerMock.Verify(manager => manager.UpdateAsync(user), Times.Once);
    }
}