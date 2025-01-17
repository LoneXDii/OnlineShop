using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Application.Exceptions;
using OrderService.Application.UseCases.CartUseCases.SetItemQuantityInCartUseCase;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;

namespace OrderService.Tests.UnitTests.UseCases.CartUseCases;

public class SetItemQuantityInCartRequestHandlerTests
{
    private readonly Mock<ITemporaryStorageService> _temporaryStorageMock = new();
    private readonly Mock<IProductService> _productServiceMock = new();
    private readonly Mock<ILogger<SetItemQuantityInCartRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly SetItemQuantityInCartRequestHandler _handler;

    public SetItemQuantityInCartRequestHandlerTests()
    {
        var cart = new Dictionary<int, ProductEntity>();
        _temporaryStorageMock.Setup(cache => cache.GetCartAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        _handler = new SetItemQuantityInCartRequestHandler(
            _temporaryStorageMock.Object,
            _productServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProductReturnedFromServiceIsNull_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<SetItemQuantityInCartRequest>();

        _productServiceMock.Setup(service => service.GetByIdIfSufficientQuantityAsync(It.IsAny<int>(), It.IsAny<int>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductEntity?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, It.IsAny<CancellationToken>()));

        //Assert
        _temporaryStorageMock.Verify(cache => cache.SaveCartAsync(It.IsAny<Dictionary<int, ProductEntity>>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenProductReturnedFromServiceIsNot_ShouldAddThisProductToCart()
    {
        //Arrange
        var request = _fixture.Create<SetItemQuantityInCartRequest>();
        var product = _fixture.Create<ProductEntity>();

        _productServiceMock.Setup(service => service.GetByIdIfSufficientQuantityAsync(It.IsAny<int>(), It.IsAny<int>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var cart = await _temporaryStorageMock.Object.GetCartAsync(default);

        //Act
        await _handler.Handle(request, It.IsAny<CancellationToken>());

        //Assert
        Assert.Equal(product, cart[product.Id]);

        _temporaryStorageMock.Verify(cache => cache.SaveCartAsync(It.IsAny<Dictionary<int, ProductEntity>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
