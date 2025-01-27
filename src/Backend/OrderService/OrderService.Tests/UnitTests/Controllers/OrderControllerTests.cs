using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using OrderService.API.Controllers;
using OrderService.Application.DTO;
using OrderService.Application.UseCases.OrderUseCases.CancelOrderUseCase;
using OrderService.Application.UseCases.OrderUseCases.CompleteOrderUseCase;
using OrderService.Application.UseCases.OrderUseCases.ConfirmOrderUseCase;
using OrderService.Application.UseCases.OrderUseCases.CreateOrderUseCase;
using OrderService.Application.UseCases.OrderUseCases.GetAllOrdersUseCase;
using OrderService.Application.UseCases.OrderUseCases.GetOrderByIdUseCase;
using OrderService.Application.UseCases.OrderUseCases.GetUserOrdersUseCase;
using OrderService.Application.UseCases.PaymentUseCases.PayOrderUseCase;

namespace OrderService.Tests.UnitTests.Controllers;

public class OrderControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly OrderController _controller;

    public OrderControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _controller = new OrderController(_mediatorMock.Object, _mapperMock.Object);
        
        var userId = "test-user-id";
        var stripeId = "test-stripe-id";
        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("Id", userId), new Claim("StripeId", stripeId)]))
        };
        _controller.ControllerContext.HttpContext = context;
    }

    [Fact]
    public async Task CreateOrder_WhenCalled_ShouldCallCreateOrderRequest()
    {
        //Act
        await _controller.CreateOrder(CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<CreateOrderRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetOrders_WhenCalled_ShouldCallGetUserOrdersRequest()
    {
        //Arrange
        var pagination = new PaginationDTO();

        _mapperMock.Setup(mapper => mapper.Map<GetUserOrdersRequest>(pagination))
            .Returns(new GetUserOrdersRequest());
        
        //Act
        await _controller.GetOrders(CancellationToken.None, pagination);

        //Assert
        _mapperMock.Verify(m => m.Map<GetUserOrdersRequest>(pagination), Times.Once);
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetUserOrdersRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetAllOrders_WhenCalled_ShouldCallGetAllOrdersRequest()
    {
        //Arrange
        var pagination = new PaginationDTO();

        //Act
        await _controller.GetAllOrders(CancellationToken.None, pagination);

        //Assert
        _mapperMock.Verify(m => m.Map<GetAllOrdersRequest>(pagination), Times.Once);
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllOrdersRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetUserOrders_WhenCalled_ShouldCallGetUserOrdersRequest()
    {
        //Arrange
        var userId = "test-user-id";
        var pagination = new PaginationDTO();

        _mapperMock.Setup(mapper => mapper.Map<GetUserOrdersRequest>(pagination))
            .Returns(new GetUserOrdersRequest());
        
        //Act
        await _controller.GetUserOrders(CancellationToken.None, userId, pagination);

        //Assert
        _mapperMock.Verify(m => m.Map<GetUserOrdersRequest>(pagination), Times.Once);
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetUserOrdersRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetOrder_WhenCalled_ShouldCallGetOrderByIdRequest()
    {
        //Arrange
        var orderId = "test-order-id";

        //Act
        await _controller.GetOrder(orderId, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetOrderByIdRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CancelOrder_WhenCalled_ShouldCallCancelOrderRequest()
    {
        //Arrange
        var orderId = "test-order-id";

        //Act
        await _controller.CancelOrder(orderId, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<CancelOrderRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ConfirmOrder_WhenCalled_ShouldCallConfirmOrderRequest()
    {
        //Arrange
        var orderId = "test-order-id";

        //Act
        await _controller.ConfirmOrder(orderId, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<ConfirmOrderRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CompleteOrder_WhenCalled_ShouldCallCompleteOrderRequest()
    {
        //Arrange
        var orderId = "test-order-id";

        //Act
        await _controller.CompleteOrder(orderId, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<CompleteOrderRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Pay_WhenCalled_ShouldCallPayOrderRequest()
    {
        //Arrange
        var orderId = "test-order-id";

        //Act
        await _controller.Pay(orderId, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<PayOrderRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetOrderAdmin_WhenCalled_ShouldCallGetOrderByIdRequest()
    {
        //Arrange
        var orderId = "test-order-id";

        //Act
        await _controller.GetOrderAdmin(orderId, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetOrderByIdRequest>(), CancellationToken.None), Times.Once);
    }
    
    [Fact]
    public async Task CancelOrderAdmin_WhenCalled_ShouldCallCancelOrderRequest()
    {
        //Arrange
        var orderId = "test-order-id";

        //Act
        await _controller.CancelOrderAdmin(orderId, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<CancelOrderRequest>(), CancellationToken.None), Times.Once);
    }
}