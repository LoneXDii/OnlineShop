using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.BLL.Exceptions;
using UserService.BLL.UseCases.UserUseCases.RemoveUserFromAdmins;
using UserService.DAL.Entities;
using UserService.Tests.Factories;

namespace UserService.Tests.UnitTests.UseCases.UserUseCases;

public class RemoveUserFromAdminsRequestHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<ILogger<RemoveUserFromAdminsRequestHandler>> _loggerMock = new();
    private readonly RemoveUserFromAdminsRequestHandler _handler;
    private readonly Fixture _fixture = new();

    public RemoveUserFromAdminsRequestHandlerTests()
    {
        _userManagerMock = MocksFactory.CreateUserManager();

        _handler = new RemoveUserFromAdminsRequestHandler(
            _userManagerMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<RemoveUserFromAdminsRequest>();
        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.UserId))
            .ReturnsAsync((AppUser?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such user", exception.Message);
        _userManagerMock.Verify(manager => manager.RemoveFromRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserExists_ShouldRemoveUserFromAdminRole()
    {
        //Arrange
        var request = _fixture.Create<RemoveUserFromAdminsRequest>();
        var user = _fixture.Create<AppUser>();

        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.UserId))
            .ReturnsAsync(user);
        _userManagerMock.Setup(manager => manager.RemoveFromRoleAsync(user, "admin"))
            .ReturnsAsync(IdentityResult.Success);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _userManagerMock.Verify(manager => manager.FindByIdAsync(request.UserId), Times.Once);
        _userManagerMock.Verify(manager => manager.RemoveFromRoleAsync(user, "admin"), Times.Once);
    }
}