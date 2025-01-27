using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using OrderService.Application.DTO;
using OrderService.Application.Exceptions;
using OrderService.Application.Settings;
using OrderService.Application.UseCases.OrderUseCases.GetUserOrdersUseCase;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;
using System.Linq.Expressions;

namespace OrderService.Tests.UnitTests.UseCases.OrderUseCases;

public class GetUserOrdersRequestHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly IOptions<PaginationSettings> _paginationOptions;
    private readonly Fixture _fixture = new();
    private readonly GetUserOrdersRequestHandler _handler;

    public GetUserOrdersRequestHandlerTests()
    {
        var paginationSettings = new PaginationSettings { MaxPageSize = 10 };
        _paginationOptions = Options.Create(paginationSettings);

        _handler = new GetUserOrdersRequestHandler(
            _orderRepositoryMock.Object,
            _mapperMock.Object,
            _paginationOptions
        );
    }

    [Fact]
    public async Task Handle_WhenRequestedPageIsGreaterThanTotalPages_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = new GetUserOrdersRequest { PageNo = 3, PageSize = 10 };

        _orderRepositoryMock.Setup(repo => repo.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(20);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such page", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ShouldReturnPaginatedListModel()
    {
        //Arrange
        var request = new GetUserOrdersRequest { PageNo = 1, PageSize = 10 };
        var orderEntities = _fixture.CreateMany<OrderEntity>(10).ToList();
        var totalItems = orderEntities.Count;

        _orderRepositoryMock.Setup(repo => repo.ListWithPaginationAsync(request.PageNo, request.PageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(orderEntities);

        _orderRepositoryMock.Setup(repo => repo.CountAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<OrderEntity, bool>>[]>()))
            .ReturnsAsync(totalItems);

        _mapperMock.Setup(m => m.Map<List<OrderDTO>>(It.IsAny<List<OrderEntity>>()))
            .Returns(new List<OrderDTO> { _fixture.Create<OrderDTO>() });

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(request.PageNo, result.CurrentPage);
        Assert.Equal(1, result.TotalPages);
        Assert.NotEmpty(result.Items);
    }

    [Fact]
    public async Task Handle_WhenPageSizeExceedsMaxPageSize_ShouldUseMaxPageSize()
    {
        //Arrange
        var maxPageSize = _paginationOptions.Value.MaxPageSize;
        var request = new GetUserOrdersRequest { PageNo = 1, PageSize = maxPageSize * 10 + 10 };

        _orderRepositoryMock.Setup(repo => repo.ListWithPaginationAsync(request.PageNo, It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<OrderEntity, bool>>[]>()))
            .ReturnsAsync((int pageNo, int pageSize, CancellationToken ct, Expression<Func<OrderEntity, bool>>[] filters) =>
                _fixture.CreateMany<OrderEntity>(pageSize).ToList());

        _orderRepositoryMock.Setup(repo => repo.CountAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<OrderEntity, bool>>[]>()))
           .ReturnsAsync(maxPageSize);

        _mapperMock.Setup(m => m.Map<List<OrderDTO>>(It.IsAny<List<OrderEntity>>()))
              .Returns((List<OrderEntity> orderEntities) => _fixture.CreateMany<OrderDTO>(orderEntities.Count).ToList());

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(maxPageSize, result.Items.Count);
    }
}
