using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.BLL.Exceptions;
using UserService.BLL.UseCases.UserUseCases.AssignToAdminRole;
using UserService.DAL.Entities;
using UserService.Tests.Factories;

namespace UserService.Tests.UnitTests.UseCases.UserUseCases;

public class AssignToAdminRoleRequestHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<ILogger<AssignToAdminRoleRequestHandler>> _loggerMock = new();
    private readonly AssignToAdminRoleRequestHandler _handler;
    private readonly Fixture _fixture = new();

    public AssignToAdminRoleRequestHandlerTests()
    {
        _userManagerMock = MocksFactory.CreateUserManager();
        _handler = new AssignToAdminRoleRequestHandler(
            _userManagerMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<AssignToAdminRoleRequest>();
        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.UserId))
            .ReturnsAsync((AppUser?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such user", exception.Message);
        _userManagerMock.Verify(manager => manager.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserExists_ShouldAssignAdminRole()
    {
        //Arrange
        var request = _fixture.Create<AssignToAdminRoleRequest>();
        var user = _fixture.Create<AppUser>();

        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.UserId))
            .ReturnsAsync(user);
        _userManagerMock.Setup(manager => manager.AddToRoleAsync(user, "admin"))
            .ReturnsAsync(IdentityResult.Success);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _userManagerMock.Verify(manager => manager.FindByIdAsync(request.UserId), Times.Once);
        _userManagerMock.Verify(manager => manager.AddToRoleAsync(user, "admin"), Times.Once);
    }
}