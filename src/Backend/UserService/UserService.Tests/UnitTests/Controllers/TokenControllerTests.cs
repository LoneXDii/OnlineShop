using MediatR;
using Moq;
using UserService.API.Controllers;
using UserService.BLL.UseCases.AuthUseCases.RefreshAccessTokenUseCase;

namespace UserService.Tests.UnitTests.Controllers;

public class TokenControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly TokenController _controller;

    public TokenControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new TokenController(_mediatorMock.Object);
    }

    [Fact]
    public async Task RefreshAccessToken_WhenCalled_ShouldCallRefreshAccessTokenRequest()
    {
        //Arrange
        var refreshToken = "test-refresh-token";

        //Act
        await _controller.RefreshAccessToken(refreshToken);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<RefreshAccessTokenRequest>(), CancellationToken.None), Times.Once);
    }
}