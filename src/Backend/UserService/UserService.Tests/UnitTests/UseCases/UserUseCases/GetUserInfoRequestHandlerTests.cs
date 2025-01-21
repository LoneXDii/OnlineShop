using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.BLL.DTO;
using UserService.BLL.Exceptions;
using UserService.BLL.UseCases.UserUseCases.GetUserInfoUseCase;
using UserService.DAL.Entities;
using UserService.Tests.Factories;

namespace UserService.Tests.UnitTests.UseCases.UserUseCases;

public class GetUserInfoRequestHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<GetUserInfoRequestHandler>> _loggerMock = new();
    private readonly GetUserInfoRequestHandler _handler;
    private readonly Fixture _fixture = new();

    public GetUserInfoRequestHandlerTests()
    {
        _userManagerMock = MocksFactory.CreateUserManager();
        _handler = new GetUserInfoRequestHandler(
            _userManagerMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<GetUserInfoRequest>();
        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.userId))
            .ReturnsAsync((AppUser?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such user", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenUserExists_ShouldReturnUserInfoDTO()
    {
        //Arrange
        var request = _fixture.Create<GetUserInfoRequest>();
        var user = _fixture.Create<AppUser>();
        var userDTO = _fixture.Create<UserInfoDTO>();

        _userManagerMock.Setup(manager => manager.FindByIdAsync(request.userId))
            .ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<UserInfoDTO>(user))
            .Returns(userDTO);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal(userDTO, result);
        _userManagerMock.Verify(manager => manager.FindByIdAsync(request.userId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<UserInfoDTO>(user), Times.Once);
    }
}