using System.Security.Claims;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using UserService.API.Controllers;
using UserService.BLL.DTO;
using UserService.BLL.UseCases.UserUseCases.AskForResetPasswordUseCase;
using UserService.BLL.UseCases.UserUseCases.AssignToAdminRole;
using UserService.BLL.UseCases.UserUseCases.GetUserInfoUseCase;
using UserService.BLL.UseCases.UserUseCases.ListUsersWithPaginationUseCase;
using UserService.BLL.UseCases.UserUseCases.RemoveUserFromAdmins;
using UserService.BLL.UseCases.UserUseCases.ResetPasswordUseCase;
using UserService.BLL.UseCases.UserUseCases.UpdateEmailUseCase;
using UserService.BLL.UseCases.UserUseCases.UpdatePasswordUseCase;
using UserService.BLL.UseCases.UserUseCases.UpdateUserUseCase;

namespace UserService.Tests.UnitTests.Controllers;

public class UsersControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly UsersController _controller;
    private readonly Fixture _fixture = new();
    
    public UsersControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new UsersController(_mediatorMock.Object);
        
        var userId = "test-user-id";
        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("Id", userId)]))
        };
        _controller.ControllerContext.HttpContext = context;
    }

    [Fact]
    public async Task UpdateUser_WhenCalled_ShouldCallUpdateUserRequest()
    {
        //Arrange
        var userDTO = new UpdateUserDTO();
        var userId = "test-user-id";

        //Act
        await _controller.UpdateUser(userDTO);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateUserRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task UpdateEmail_WhenCalled_ShouldCallUpdateEmailRequest()
    {
        //Arrange
        var newEmail = new EmailDTO { Email = "test@example.com" };
        var userId = "test-user-id";

        //Act
        await _controller.UpdateEmail(newEmail);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateEmailRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task UpdatePassword_WhenCalled_ShouldCallUpdatePasswordRequest()
    {
        //Arrange
        var updatePasswordDTO = new UpdatePasswordDTO();
        var userId = "test-user-id";

        //Act
        await _controller.UpdatePassword(updatePasswordDTO);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<UpdatePasswordRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ListUsers_WhenCalled_ShouldCallListUsersWithPaginationRequest()
    {
        //Arrange
        var pagination = new PaginationDTO();

        //Act
        await _controller.ListUsers(pagination);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<ListUsersWithPaginationRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetUserInfo_WhenCalled_ShouldCallGetUserInfoRequest()
    {
        //Arrange
        var userId = "test-user-id";

        //Act
        await _controller.GetUserInfo();

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetUserInfoRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task AskForResetPassword_WhenCalled_ShouldCallAskForResetPasswordRequest()
    {
        //Arrange
        var request = _fixture.Create<AskForResetPasswordRequest>();

        //Act
        await _controller.AskForResetPassword(request);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ResetPassword_WhenCalled_ShouldCallResetPasswordRequest()
    {
        //Arrange
        var request = _fixture.Create<ResetPasswordRequest>();

        //Act
        await _controller.ResetPassword(request);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task AssignUserToAdmin_WhenCalled_ShouldCallAssignToAdminRoleRequest()
    {
        //Arrange
        var request = _fixture.Create<AssignToAdminRoleRequest>();
        var userId = "test-user-id";

        //Act
        await _controller.AssignUserToAdmin(request, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task DeleteUserFromAdmins_WhenCalled_ShouldCallRemoveUserFromAdminsRequest()
    {
        //Arrange
        var request = _fixture.Create<RemoveUserFromAdminsRequest>();
        
        //Act
        await _controller.DeleteUserFromAdmins(request, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }
    
    [Fact]
    public async Task GetUserInfoAdmin_WhenCalled_ShouldCallGetUserInfoRequest()
    {
        //Arrange
        var userId = "test-user-id";
        
        //Act
        await _controller.GetUserInfo(userId);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetUserInfoRequest>(), CancellationToken.None), Times.Once);
    }
}