using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using UserService.API.Controllers;
using UserService.BLL.DTO;
using UserService.BLL.UseCases.AuthUseCases.EmailConfirmationUseCase;
using UserService.BLL.UseCases.AuthUseCases.LoginUserUseCase;
using UserService.BLL.UseCases.AuthUseCases.LogoutUserUseCase;
using UserService.BLL.UseCases.AuthUseCases.RegisterUserUseCase;
using UserService.BLL.UseCases.AuthUseCases.ResendEmailConfirmationCodeUseCase;

namespace UserService.Tests.UnitTests.Controllers;

public class AccountControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AccountController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Register_WhenCalled_ShouldCallRegisterUserRequest()
    {
        //Arrange
        var registerModel = new RegisterDTO();

        //Act
        await _controller.Register(registerModel);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<RegisterUserRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Login_WhenCalled_ShouldCallLoginUserRequest()
    {
        //Arrange
        var loginModel = new LoginDTO();

        //Act
        await _controller.Login(loginModel);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<LoginUserRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ConfirmEmail_WhenCalled_ShouldCallEmailConfirmationRequest()
    {
        //Arrange
        var email = "test@example.com";
        var code = "confirmation-code";

        //Act
        await _controller.ConfirmEmail(email, code);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<EmailConfirmationRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ResendEmailConfirmation_WhenCalled_ShouldCallResendEmailConfirmationCodeRequest()
    {
        //Arrange
        var request = new ResendEmailConfirmationCodeRequest("qwe");

        //Act
        await _controller.ResendEmailConfirmation(request);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Logout_WhenCalled_ShouldCallLogoutUserRequest()
    {
        //Arrange
        var userId = "test-user-id";
        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("Id", userId)]))
        };
        _controller.ControllerContext.HttpContext = context;

        //Act
        await _controller.Logout();

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<LogoutUserRequest>(), CancellationToken.None), Times.Once);
    }
}