using AutoFixture;
using Moq;
using OrderService.Application.UseCases.CartUseCases.RemoveItemFromCartUseCase;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;

namespace OrderService.Tests.UnitTests.UseCases.CartUseCases;

public class RemoveItemFromCartRequestHandlerTests
{
    private readonly Mock<ITemporaryStorageService> _temporaryStorageMock = new();
    private readonly Fixture _fixture = new();
    private readonly RemoveItemFromCartRequestHandler _handler;

    public RemoveItemFromCartRequestHandlerTests()
    {
        var product = _fixture.Build<ProductEntity>()
            .With(p => p.Id, 1).Create();

        var cart = new Dictionary<int, ProductEntity>
        {
            { product.Id, product }
        };

        _temporaryStorageMock.Setup(cache => cache.GetCartAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        _handler = new RemoveItemFromCartRequestHandler(_temporaryStorageMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRequested_ShouldRemoveItemFromCart()
    {
        //Arrange
        var request = new RemoveItemFromCartRequest(1);

        var cart = await _temporaryStorageMock.Object.GetCartAsync(default);

        //Act
        await _handler.Handle(request, It.IsAny<CancellationToken>());

        //Assert
        Assert.Empty(cart);

        _temporaryStorageMock.Verify(cache => cache.SaveCartAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
    }
}
