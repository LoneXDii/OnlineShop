using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Application.Exceptions;
using OrderService.Application.UseCases.PaymentUseCases.PayOrderUseCase;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Abstractions.Payments;
using OrderService.Domain.Common.Statuses;
using OrderService.Domain.Entities;

namespace OrderService.Tests.UnitTests.UseCases.PaymentUseCases;

public class PayOrderRequestHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IPaymentService> _paymentServiceMock = new();
    private readonly Mock<ILogger<PayOrderRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly PayOrderRequestHandler _handler;

    public PayOrderRequestHandlerTests()
    {
        _handler = new PayOrderRequestHandler(
            _orderRepositoryMock.Object,
            _paymentServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenOrderNotFound_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<PayOrderRequest>();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((OrderEntity?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such order", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotHaveAccess_ShouldThrowAccessDeniedException()
    {
        //Arrange
        var request = _fixture.Create<PayOrderRequest>();
        var order = _fixture.Build<OrderEntity>()
            .With(o => o.UserId, $"{request.userId}qwe")
            .Create();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        //Act
        var exception = await Assert.ThrowsAsync<AccessDeniedException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("You have no access to this order", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenOrderIsAlreadyPaid_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<PayOrderRequest>();
        var order = _fixture.Build<OrderEntity>()
            .With(o => o.UserId, request.userId)
            .With(o => o.PaymentStatus, PaymentStatuses.Paid) 
            .Create();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("This order is already paid", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldReturnPaymentResult()
    {
        //Arrange
        var request = _fixture.Create<PayOrderRequest>();
        var order = _fixture.Build<OrderEntity>()
            .With(o => o.UserId, request.userId)
            .With(o => o.PaymentStatus, PaymentStatuses.NotPaid) 
            .Create();
        var paymentResult = _fixture.Create<string>();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        _paymentServiceMock.Setup(service => service.PayAsync(order, request.stribeId))
            .ReturnsAsync(paymentResult);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal(paymentResult, result);
        _paymentServiceMock.Verify(service => service.PayAsync(order, request.stribeId), Times.Once);
    }
}
