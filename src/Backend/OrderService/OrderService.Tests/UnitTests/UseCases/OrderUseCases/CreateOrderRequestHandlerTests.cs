using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Application.DTO;
using OrderService.Application.Exceptions;
using OrderService.Application.UseCases.OrderUseCases.CreateOrderUseCase;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;

namespace OrderService.Tests.UnitTests.UseCases.OrderUseCases;

public class CreateOrderRequestHandlerTests
{
    private readonly Mock<ITemporaryStorageService> _temporaryStorageMock = new();
    private readonly Mock<IProductService> _productServiceMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IProducerService> _producerServiceMock = new();
    private readonly Mock<ILogger<CreateOrderRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly CreateOrderRequestHandler _handler;

    public CreateOrderRequestHandlerTests()
    {
        _handler = new CreateOrderRequestHandler(
            _temporaryStorageMock.Object,
            _productServiceMock.Object,
            _orderRepositoryMock.Object,
            _mapperMock.Object,
            _producerServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenCartIsEmpty_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<CreateOrderRequest>();
        _temporaryStorageMock.Setup(storage => storage.GetCartAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<int, ProductEntity>());

        _mapperMock.Setup(mapper => mapper.Map<CartDTO>(It.IsAny<Dictionary<int, ProductEntity>>()))
            .Returns(_fixture.Build<CartDTO>()
                .With(cart => cart.Products, new List<ProductEntity>())
                .Create());

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("Your cart is empty", exception.Message);
        _orderRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), 
            Times.Never);
        _producerServiceMock.Verify(service => service.ProduceOrderActionsAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), 
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenProductsServiceReturnsNull_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<CreateOrderRequest>();
        var cart = new Dictionary<int, ProductEntity>
        {
            { 1, _fixture.Create<ProductEntity>() }
        };
        var cartDto = new CartDTO { Products = cart.Values.ToList(), TotalCost = 100 };

        _temporaryStorageMock.Setup(storage => storage.GetCartAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        _mapperMock.Setup(m => m.Map<CartDTO>(It.IsAny<Dictionary<int, ProductEntity>>()))
            .Returns(cartDto);

        _productServiceMock.Setup(service => service.TakeProductsIfSufficientQuantityAsync(cartDto.Products, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<ProductEntity>?)null);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("Not enought products in stock", exception.Message);
        _orderRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), 
            Times.Never);
        _producerServiceMock.Verify(service => service.ProduceOrderActionsAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), 
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldCreateOrder()
    {
        //Arrange
        var request = _fixture.Create<CreateOrderRequest>();
        var cart = new Dictionary<int, ProductEntity>
        {
            { 1, _fixture.Create<ProductEntity>() }
        };
        var cartDto = new CartDTO { Products = cart.Values.ToList(), TotalCost = 100 };
        var orderProducts = new List<ProductEntity> { _fixture.Create<ProductEntity>() };

        _temporaryStorageMock.Setup(storage => storage.GetCartAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        _mapperMock.Setup(m => m.Map<CartDTO>(It.IsAny<Dictionary<int, ProductEntity>>()))
            .Returns(cartDto);

        _productServiceMock.Setup(service => service.TakeProductsIfSufficientQuantityAsync(cartDto.Products, It.IsAny<CancellationToken>()))
            .ReturnsAsync(orderProducts);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _orderRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), 
            Times.Once);
        _producerServiceMock.Verify(service => service.ProduceOrderActionsAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), 
            Times.Once);
        _temporaryStorageMock.Verify(storage => storage.SaveCartAsync(It.IsAny<Dictionary<int, ProductEntity>>(), It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}
