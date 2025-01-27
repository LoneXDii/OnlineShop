using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Application.Exceptions;
using OrderService.Application.UseCases.OrderUseCases.CancelOrderUseCase;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Common.Statuses;
using OrderService.Domain.Entities;

namespace OrderService.Tests.UnitTests.UseCases.OrderUseCases;

public class CancelOrderRequestHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IProductService> _productServiceMock = new();
    private readonly Mock<IProducerService> _producerServiceMock = new();
    private readonly Mock<ILogger<CancelOrderRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly CancelOrderRequestHandler _handler;

    public CancelOrderRequestHandlerTests()
    {
        _handler = new CancelOrderRequestHandler(
            _orderRepositoryMock.Object,
            _productServiceMock.Object,
            _producerServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenOrderNotFound_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<CancelOrderRequest>();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((OrderEntity?)null);

        //Act 
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such order", exception.Message);
        _orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _productServiceMock.Verify(service => service.ReturnProductsAsync(It.IsAny<List<ProductEntity>>(), It.IsAny<CancellationToken>()), Times.Never);
        _producerServiceMock.Verify(service => service.ProduceOrderActionsAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserHasNoAccess_ShouldThrowAccessDeniedException()
    {
        //Arrange
        var request = _fixture.Create<CancelOrderRequest>();
        var order = _fixture.Build<OrderEntity>()
            .With(o => o.UserId, $"{request.userId}qwe")
            .Create();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        //Act
        var exception = await Assert.ThrowsAsync<AccessDeniedException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("You dont have access to this order", exception.Message);
        _orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _productServiceMock.Verify(service => service.ReturnProductsAsync(It.IsAny<List<ProductEntity>>(), It.IsAny<CancellationToken>()), Times.Never);
        _producerServiceMock.Verify(service => service.ProduceOrderActionsAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenOrderIsCompleted_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<CancelOrderRequest>();
        var order = _fixture.Build<OrderEntity>()
            .With(o => o.OrderStatus, OrderStatuses.Completed)
            .With(o => o.UserId, request.userId)
            .Create();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("Cannot cancel completed order", exception.Message);
        _orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _productServiceMock.Verify(service => service.ReturnProductsAsync(It.IsAny<List<ProductEntity>>(), It.IsAny<CancellationToken>()), Times.Never);
        _producerServiceMock.Verify(service => service.ProduceOrderActionsAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenOrderIsCancelled_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<CancelOrderRequest>();
        var order = _fixture.Build<OrderEntity>()
            .With(o => o.OrderStatus, OrderStatuses.Cancelled)
            .With(o => o.UserId, request.userId)
            .Create();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("This order is already cancelled", exception.Message);
        _orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _productServiceMock.Verify(service => service.ReturnProductsAsync(It.IsAny<List<ProductEntity>>(), It.IsAny<CancellationToken>()), Times.Never);
        _producerServiceMock.Verify(service => service.ProduceOrderActionsAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldCancelOrder()
    {
        //Arrange
        var request = _fixture.Create<CancelOrderRequest>();
        var order = _fixture.Build<OrderEntity>()
            .With(o => o.OrderStatus, OrderStatuses.Created)
            .With(o => o.UserId, request.userId)
            .Create();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal(OrderStatuses.Cancelled, order.OrderStatus);
        _orderRepositoryMock.Verify(repo => repo.UpdateAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        _productServiceMock.Verify(service => service.ReturnProductsAsync(order.Products, It.IsAny<CancellationToken>()), Times.Once);
        _producerServiceMock.Verify(service => service.ProduceOrderActionsAsync(order, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserIdIsNull_ShouldCancelOrder()
    {
        //Arrange
        var request = new CancelOrderRequest("1", null);
        var order = _fixture.Build<OrderEntity>()
            .With(o => o.OrderStatus, OrderStatuses.Created)
            .With(o => o.UserId, request.userId)
            .Create();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal(OrderStatuses.Cancelled, order.OrderStatus);
        _orderRepositoryMock.Verify(repo => repo.UpdateAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        _productServiceMock.Verify(service => service.ReturnProductsAsync(order.Products, It.IsAny<CancellationToken>()), Times.Once);
        _producerServiceMock.Verify(service => service.ProduceOrderActionsAsync(order, It.IsAny<CancellationToken>()), Times.Once);
    }
}
