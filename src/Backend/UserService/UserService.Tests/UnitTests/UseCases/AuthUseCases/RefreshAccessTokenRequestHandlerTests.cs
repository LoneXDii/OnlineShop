using AutoFixture;
using Moq;
using UserService.BLL.Exceptions;
using UserService.BLL.UseCases.AuthUseCases.RefreshAccessTokenUseCase;
using UserService.DAL.Services.Authentication;

namespace UserService.Tests.UnitTests.UseCases.AuthUseCases;

public class RefreshAccessTokenRequestHandlerTests
{
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly RefreshAccessTokenRequestHandler _handler;
    private readonly Fixture _fixture = new();

    public RefreshAccessTokenRequestHandlerTests()
    {
        _handler = new RefreshAccessTokenRequestHandler(_tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRefreshTokenIsInvalid_ShouldThrowInvalidTokenException()
    {
        //Arrange
        var request = _fixture.Create<RefreshAccessTokenRequest>();
        _tokenServiceMock.Setup(service => service.RefreshAccessTokenAsync(request.refreshToken))
            .ReturnsAsync((string?)null);

        //Act
        var exception = await Assert.ThrowsAsync<InvalidTokenException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("Invalid refresh token", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenRefreshTokenIsValid_ShouldReturnNewAccessToken()
    {
        //Arrange
        var request = _fixture.Create<RefreshAccessTokenRequest>();
        var newAccessToken = _fixture.Create<string>();

        _tokenServiceMock.Setup(service => service.RefreshAccessTokenAsync(request.refreshToken))
            .ReturnsAsync(newAccessToken); 

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal(newAccessToken, result);
    }
}