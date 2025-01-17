using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Application.Exceptions;
using OrderService.Application.UseCases.PaymentUseCases.ConfirmPaymentUseCase;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Abstractions.Payments;
using OrderService.Domain.Common.Statuses;
using OrderService.Domain.Entities;

namespace OrderService.Tests.UnitTests.UseCases.PaymentUseCases;

public class ConfirmPaymentRequestHandlerTests
{
    private readonly Mock<IPaymentService> _paymentServiceMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<ILogger<ConfirmPaymentRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly ConfirmPaymentRequestHandler _handler;

    public ConfirmPaymentRequestHandlerTests()
    {
        _handler = new ConfirmPaymentRequestHandler(
            _paymentServiceMock.Object,
            _orderRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenPaymentServiceReturnsNull_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<ConfirmPaymentRequest>();
        _paymentServiceMock.Setup(service => service.GetSuccessPaymentOrderId(request.json, request.signature))
            .Returns((string?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such order", exception.Message);
        _orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenOrderNotFound_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<ConfirmPaymentRequest>();
        var orderId = _fixture.Create<string>();

        _paymentServiceMock.Setup(service => service.GetSuccessPaymentOrderId(request.json, request.signature))
            .Returns(orderId);
        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((OrderEntity?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such order", exception.Message);
        _orderRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), 
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenOrderIsFound_ShouldUpdatePaymentStatus()
    {
        //Arrange
        var request = _fixture.Create<ConfirmPaymentRequest>();
        var orderId = _fixture.Create<string>();
        var order = _fixture.Create<OrderEntity>();

        _paymentServiceMock.Setup(service => service.GetSuccessPaymentOrderId(request.json, request.signature))
            .Returns(orderId);
        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal(PaymentStatuses.Paid, order.PaymentStatus);
        _orderRepositoryMock.Verify(repo => repo.UpdateAsync(order, It.IsAny<CancellationToken>()), Times.Once);
    }
}
