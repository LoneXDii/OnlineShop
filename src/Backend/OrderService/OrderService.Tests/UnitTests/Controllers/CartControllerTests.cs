using MediatR;
using Moq;
using OrderService.API.Controllers;
using OrderService.Application.DTO;
using OrderService.Application.UseCases.CartUseCases.AddProductToCartUseCase;
using OrderService.Application.UseCases.CartUseCases.ClearCartUseCase;
using OrderService.Application.UseCases.CartUseCases.GetCartUseCase;
using OrderService.Application.UseCases.CartUseCases.ReduceItemQuantityInCartUseCase;
using OrderService.Application.UseCases.CartUseCases.RemoveItemFromCartUseCase;
using OrderService.Application.UseCases.CartUseCases.SetItemQuantityInCartUseCase;

namespace OrderService.Tests.UnitTests.Controllers;

public class CartControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CartController _controller;

    public CartControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new CartController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetCart_WhenCalled_ShouldCallGetCartRequest()
    {
        //Act
        await _controller.GetCart(CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetCartRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task AddToCart_WhenCalled_ShouldCallAddProductToCartRequest()
    {
        //Arrange
        var product = new CartProductDTO();

        //Act
        await _controller.AddToCart(product, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<AddProductToCartRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task SetQuantity_WhenCalled_ShouldCallSetItemQuantityInCartRequest()
    {
        //Arrange
        var productId = 1;
        var quantity = new QuantityDTO { Quantity = 5 };

        //Act
        await _controller.SetQuantity(productId, quantity, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<SetItemQuantityInCartRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ReduceQuantity_WhenCalled_ShouldCallReduceItemsInCartRequest()
    {
        //Arrange
        var productId = 1;
        var quantity = new QuantityDTO { Quantity = 2 };

        //Act
        await _controller.ReduceQuantity(productId, quantity, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<ReduceItemsInCartRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task RemoveItemFromCart_WhenCalled_ShouldCallRemoveItemFromCartRequest()
    {
        //Arrange
        var productId = 1;

        //Act
        await _controller.RemoveItemFromCart(productId, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<RemoveItemFromCartRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ClearCart_WhenCalled_ShouldCallClearCartRequest()
    {
        //Act
        await _controller.ClearCart(CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<ClearCartRequest>(), CancellationToken.None), Times.Once);
    }
}