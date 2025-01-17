using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Application.Exceptions;
using OrderService.Application.UseCases.OrderUseCases.ConfirmOrderUseCase;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Common.Statuses;
using OrderService.Domain.Entities;

namespace OrderService.Tests.UnitTests.UseCases.OrderUseCases;

public class ConfirmOrderRequestHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IProducerService> _producerServiceMock = new();
    private readonly Mock<ILogger<ConfirmOrderRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly ConfirmOrderRequestHandler _handler;

    public ConfirmOrderRequestHandlerTests()
    {
        _handler = new ConfirmOrderRequestHandler(
            _orderRepositoryMock.Object,
            _producerServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenOrderNotFound_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<ConfirmOrderRequest>();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((OrderEntity?)null);

        //Act 
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such order", exception.Message);
        _orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _producerServiceMock.Verify(service => service.ProduceOrderActionsAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenOrderStatusIsNotCreated_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<ConfirmOrderRequest>();
        var order = _fixture.Build<OrderEntity>()
            .With(o => o.OrderStatus, OrderStatuses.Completed)
            .Create();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("Order confirmation error, invalid order status", exception.Message);
        _orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _producerServiceMock.Verify(service => service.ProduceOrderActionsAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldConfirmOrder()
    {
        //Arrange
        var request = _fixture.Create<ConfirmOrderRequest>();
        var order = _fixture.Build<OrderEntity>()
            .With(o => o.OrderStatus, OrderStatuses.Created)
            .Create();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal(OrderStatuses.Confirmed, order.OrderStatus);
        _orderRepositoryMock.Verify(repo => repo.UpdateAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        _producerServiceMock.Verify(service => service.ProduceOrderActionsAsync(order, It.IsAny<CancellationToken>()), Times.Once);
    }
}
