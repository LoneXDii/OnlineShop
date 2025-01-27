using OrderService.Domain.Abstractions.Data;
using Moq;
using Microsoft.Extensions.Logging;
using OrderService.Application.UseCases.CartUseCases.AddProductToCartUseCase;
using OrderService.Domain.Entities;
using AutoFixture;
using OrderService.Application.Exceptions;

namespace OrderService.Tests.UnitTests.UseCases.CartUseCases;

public class AddProductToCartRequestHandlerTests
{
    private readonly Mock<ITemporaryStorageService> _temporaryStorageMock = new();
    private readonly Mock<IProductService> _productServiceMock = new();
    private readonly Mock<ILogger<AddProductToCartRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly AddProductToCartRequestHandler _handler;

    public AddProductToCartRequestHandlerTests()
    {
        var cart = new Dictionary<int, ProductEntity>();
        _temporaryStorageMock.Setup(cache => cache.GetCartAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        _handler = new AddProductToCartRequestHandler(
            _temporaryStorageMock.Object,
            _productServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProductIsNull_ShouldThrowNotFoundExceptionWithCorrectErrorMessage()
    {
        //Arrange
        var request = _fixture.Create<AddProductToCartRequest>();

        _productServiceMock.Setup(service => service.GetByIdIfSufficientQuantityAsync(It.IsAny<int>(), It.IsAny<int>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductEntity?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, default));

        var cart = await _temporaryStorageMock.Object.GetCartAsync();

        //Assert
        Assert.Equal("Cannot add to cart, this product not exist or its quantity to low", exception.Message);
        Assert.Empty(cart);

        _temporaryStorageMock.Verify(cache => cache.SaveCartAsync(It.IsAny<Dictionary<int, ProductEntity>>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenProductIsNotNull_ShouldAddThisProductToCart()
    {
        //Arrange
        var request = _fixture.Create<AddProductToCartRequest>();
        var product = _fixture.Create<ProductEntity>();

        _productServiceMock.Setup(service => service.GetByIdIfSufficientQuantityAsync(It.IsAny<int>(), It.IsAny<int>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        //Act
        await _handler.Handle(request, default);

        var cart = await _temporaryStorageMock.Object.GetCartAsync();

        //Assert
        _temporaryStorageMock.Verify(cache => cache.SaveCartAsync(It.IsAny<Dictionary<int, ProductEntity>>(),
            It.IsAny<CancellationToken>()), Times.Once);

        Assert.Equal(product, cart[product.Id]);
    }
}
