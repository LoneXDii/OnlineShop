using AutoFixture;
using AutoMapper;
using Moq;
using OrderService.Application.DTO;
using OrderService.Application.UseCases.CartUseCases.GetCartUseCase;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;

namespace OrderService.Tests.UnitTests.UseCases.CartUseCases;

public class GetCartRequestHandlerTests
{
    private readonly Mock<ITemporaryStorageService> _temporaryStorageMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Fixture _fixture = new();
    private readonly GetCartRequestHandler _handler;

    public GetCartRequestHandlerTests()
    {
        _temporaryStorageMock.Setup(cache => cache.GetCartAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<int, ProductEntity>());

        _mapperMock.Setup(mapper => mapper.Map<CartDTO>(It.IsAny<Dictionary<int, ProductEntity>>()))
            .Returns(_fixture.Create<CartDTO>());

        _handler = new GetCartRequestHandler(_temporaryStorageMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRequested_ShouldReturnCartDto()
    {
        //Arrange
        var request = _fixture.Create<GetCartRequest>();

        //Act
        var result = await _handler.Handle(request, default);

        //Assert
        Assert.NotNull(result);
    }
}
