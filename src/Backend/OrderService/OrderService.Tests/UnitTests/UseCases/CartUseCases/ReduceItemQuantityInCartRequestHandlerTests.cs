using AutoFixture;
using Moq;
using OrderService.Application.UseCases.CartUseCases.ReduceItemQuantityInCartUseCase;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;

namespace OrderService.Tests.UnitTests.UseCases.CartUseCases;

public class ReduceItemQuantityInCartRequestHandlerTests
{
    private readonly Mock<ITemporaryStorageService> _temporaryStorageMock = new();
    private readonly Fixture _fixture = new();
    private readonly ReduceItemQuantityInCartRequestHandler _handler;
    private readonly int _quantityInCart = 5;

    public ReduceItemQuantityInCartRequestHandlerTests()
    {
        var cart = new Dictionary<int, ProductEntity>();
        var product = _fixture.Build<ProductEntity>()
            .With(product => product.Quantity, _quantityInCart)
            .Create();

        cart.Add(1, product);

        _temporaryStorageMock.Setup(cache => cache.GetCartAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        _handler = new ReduceItemQuantityInCartRequestHandler(_temporaryStorageMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRequestedQuantityLessThanQuantityInCart_ShouldRequceQuantityInCartAndSave()
    {
        //Arrange
        var request = new ReduceItemsInCartRequest(1, _quantityInCart - 1);
        var cart = await _temporaryStorageMock.Object.GetCartAsync(default);

        //Act
        await _handler.Handle(request, default);

        //Assert
        Assert.Equal(1, cart[1].Quantity);
        _temporaryStorageMock.Verify(cache => cache.SaveCartAsync(It.IsAny<Dictionary<int, ProductEntity>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRequestedQuantityEqualToQuantityInCart_ShouldRemoveItemFromCart()
    {
        //Arrange
        var request = new ReduceItemsInCartRequest(1, _quantityInCart);

        var cart = await _temporaryStorageMock.Object.GetCartAsync(default);

        //Act
        await _handler.Handle(request, default);

        //Assert
        Assert.Empty(cart);
        _temporaryStorageMock.Verify(cache => cache.SaveCartAsync(It.IsAny<Dictionary<int, ProductEntity>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRequestedQuantityGreaterThanQuantityInCart_ShouldRemoveItemFromCart()
    {
        //Arrange
        var request = new ReduceItemsInCartRequest(1, _quantityInCart * 2);

        var cart = await _temporaryStorageMock.Object.GetCartAsync(default);

        //Act
        await _handler.Handle(request, default);

        //Assert
        Assert.Empty(cart);
        _temporaryStorageMock.Verify(cache => cache.SaveCartAsync(It.IsAny<Dictionary<int, ProductEntity>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
