using AutoFixture;
using Moq;
using OrderService.Application.UseCases.CartUseCases.ClearCartUseCase;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;

namespace OrderService.Tests.UnitTests.UseCases.CartUseCases;

public class ClearCartRequestHandlerTests
{
    private readonly Mock<ITemporaryStorageService> _temporaryStorageMock = new();
    private readonly Fixture _fixture = new();
    private readonly ClearCartRequestHandler _handler;

    public ClearCartRequestHandlerTests()
    {
        _temporaryStorageMock.Setup(cache => cache.GetCartAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<int, ProductEntity>());

        _handler = new ClearCartRequestHandler(_temporaryStorageMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRequested_ShouldClearCartAndSaveInCache()
    {
        //Arrange
        var request = _fixture.Create<ClearCartRequest>();

        //Act
        await _handler.Handle(request, default);

        //Assert
        _temporaryStorageMock.Verify(cache => cache.SaveCartAsync(It.IsAny<Dictionary<int, ProductEntity>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
