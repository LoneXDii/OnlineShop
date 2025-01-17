using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Application.DTO;
using OrderService.Application.Exceptions;
using OrderService.Application.UseCases.OrderUseCases.GetOrderByIdUseCase;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;

namespace OrderService.Tests.UnitTests.UseCases.OrderUseCases;

public class GetOrderByIdRequestHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<GetOrderByIdRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly GetOrderByIdRequestHandler _handler;

    public GetOrderByIdRequestHandlerTests()
    {
        _handler = new GetOrderByIdRequestHandler(
            _orderRepositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenOrderNotFound_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<GetOrderByIdRequest>();
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
        var request = _fixture.Create<GetOrderByIdRequest>();
        var order = _fixture.Build<OrderEntity>()
            .With(o => o.UserId, $"{request.userId}qwe") 
            .Create();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        //Act
        var exception = await Assert.ThrowsAsync<AccessDeniedException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("You dont have access to this order", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldReturnOrderDTO()
    {
        //Arrange
        var request = _fixture.Create<GetOrderByIdRequest>();
        var order = _fixture.Build<OrderEntity>()
            .With(o => o.UserId, request.userId)
            .Create();
        var orderDto = _fixture.Create<OrderDTO>();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        _mapperMock.Setup(m => m.Map<OrderDTO>(order)).Returns(orderDto);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(orderDto, result);
    }

    [Fact]
    public async Task Handle_WhenUserIdIsNull_ShouldReturnOrderDTO()
    {
        //Arrange
        var request = new GetOrderByIdRequest("1", null);
        var order = _fixture.Create<OrderEntity>();
        var orderDto = _fixture.Create<OrderDTO>();

        _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(request.orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        _mapperMock.Setup(m => m.Map<OrderDTO>(order)).Returns(orderDto);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(orderDto, result);
    }
}
