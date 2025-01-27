using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.BLL.Exceptions;
using UserService.BLL.UseCases.AuthUseCases.LoginUserUseCase;
using UserService.DAL.Entities;
using UserService.DAL.Models;
using UserService.DAL.Services.Authentication;
using UserService.Tests.Factories;

namespace UserService.Tests.UnitTests.UseCases.AuthUseCases;

public class LoginUserRequestHandlerTests
{
    private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<ILogger<LoginUserRequestHandler>> _loggerMock = new();
    private readonly LoginUserRequestHandler _handler;
    private readonly Fixture _fixture = new();

    public LoginUserRequestHandlerTests()
    {
        _userManagerMock = MocksFactory.CreateUserManager();
        _signInManagerMock = MocksFactory.CreateSignInManager();

        _handler = new LoginUserRequestHandler(
            _signInManagerMock.Object,
            _userManagerMock.Object,
            _tokenServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCredentialsAreIncorrect_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<LoginUserRequest>();
        _signInManagerMock.Setup(manager => manager.PasswordSignInAsync(
            request.LoginModel.Email, 
            request.LoginModel.Password, 
            false, 
            false))
            .ReturnsAsync(SignInResult.Failed);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("Incorrect email or password", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenEmailIsNotConfirmed_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<LoginUserRequest>();
        var user = _fixture.Create<AppUser>();
        
        _signInManagerMock.Setup(manager => manager.PasswordSignInAsync(
            request.LoginModel.Email, 
            request.LoginModel.Password, 
            false, 
            false))
            .ReturnsAsync(SignInResult.Success);

        _userManagerMock.Setup(userManager => userManager.FindByEmailAsync(request.LoginModel.Email))
            .ReturnsAsync(user);
        
        user.EmailConfirmed = false;

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("Email is not verified", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenLoginIsSuccessful_ShouldReturnTokens()
    {
        //Arrange
        var request = _fixture.Create<LoginUserRequest>();
        var user = _fixture.Create<AppUser>();
        var tokens = _fixture.Create<TokensDTO>();

        _signInManagerMock.Setup(manager => manager.PasswordSignInAsync(
            request.LoginModel.Email, 
            request.LoginModel.Password, 
            false, 
            false))
            .ReturnsAsync(SignInResult.Success);

        _userManagerMock.Setup(userManager => userManager.FindByEmailAsync(request.LoginModel.Email))
            .ReturnsAsync(user);
        
        user.EmailConfirmed = true;

        _tokenServiceMock.Setup(service => service.GetTokensAsync(user))
            .ReturnsAsync(tokens);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal(tokens, result);
    }
}