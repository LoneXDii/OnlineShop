using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.BLL.Exceptions;
using UserService.BLL.UseCases.UserUseCases.UpdatePasswordUseCase;
using UserService.DAL.Entities;
using UserService.DAL.Services.Authentication;
using UserService.Tests.Factories;

namespace UserService.Tests.UnitTests.UseCases.UserUseCases;

public class UpdatePasswordRequestHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<ILogger<UpdatePasswordRequestHandler>> _loggerMock = new();
    private readonly UpdatePasswordRequestHandler _handler;
    private readonly Fixture _fixture;

    public UpdatePasswordRequestHandlerTests()
    {
        _userManagerMock = MocksFactory.CreateUserManager();
        
        _handler = new UpdatePasswordRequestHandler(
            _userManagerMock.Object,
            _tokenServiceMock.Object,
            _loggerMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<UpdatePasswordRequest>();
        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.userId))
            .ReturnsAsync((AppUser?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such user", exception.Message);
        _userManagerMock.Verify(manager => manager.ChangePasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _tokenServiceMock.Verify(tokenService => tokenService.InvalidateRefreshTokenAsync(It.IsAny<AppUser>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPasswordChangeFails_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<UpdatePasswordRequest>();
        var user = _fixture.Create<AppUser>();
        var errors = new List<IdentityError> { new IdentityError { Description = "Invalid password" } };

        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.userId))
            .ReturnsAsync(user);
        _userManagerMock.Setup(manager => manager.ChangePasswordAsync(user, request.updatePasswordDTO.OldPassword, request.updatePasswordDTO.NewPassword))
            .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Contains("Invalid password", exception.Message);
        _tokenServiceMock.Verify(tokenService => tokenService.InvalidateRefreshTokenAsync(It.IsAny<AppUser>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPasswordIsUpdatedSuccessfully_ShouldInvalidateTokenAndReturnRefreshToken()
    {
        //Arrange
        var request = _fixture.Create<UpdatePasswordRequest>();
        var user = _fixture.Create<AppUser>();
        var refreshToken = "new_refresh_token";

        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.userId))
            .ReturnsAsync(user);
        _userManagerMock.Setup(manager => manager.ChangePasswordAsync(user, request.updatePasswordDTO.OldPassword, request.updatePasswordDTO.NewPassword))
            .ReturnsAsync(IdentityResult.Success);
        _tokenServiceMock.Setup(tokenService => tokenService.InvalidateRefreshTokenAsync(user))
            .Returns(Task.CompletedTask);
        _tokenServiceMock.Setup(tokenService => tokenService.GetRefreshTokenAsync(user))
            .ReturnsAsync(refreshToken);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal(refreshToken, result);
        _userManagerMock.Verify(manager => manager.ChangePasswordAsync(user, request.updatePasswordDTO.OldPassword, request.updatePasswordDTO.NewPassword), Times.Once);
        _tokenServiceMock.Verify(tokenService => tokenService.InvalidateRefreshTokenAsync(user), Times.Once);
    }
}