using AutoFixture;
using AutoMapper;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Mapping;
using OrderService.Infrastructure.Models;

namespace OrderService.Tests.UnitTests.MappingProfiles.Infrastructure;

public class OrderMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public OrderMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<OrderMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenOrderEntityIsNotNull_ShouldReturnCorrectOrder()
    {
        //Arrange
        var orderEntity = _fixture.Create<OrderEntity>();

        //Act
        var order = _mapper.Map<Order>(orderEntity);

        //Assert
        Assert.NotNull(order);
        Assert.Equivalent(orderEntity, order);
    }

    [Fact]
    public void Map_WhenOrderIsNotNull_ShouldReturnCorrectOrderEntity()
    {
        //Arrange
        var order = _fixture.Create<Order>();

        //Act
        var orderEntity = _mapper.Map<OrderEntity>(order);

        //Assert
        Assert.NotNull(orderEntity);
        Assert.Equivalent(order, orderEntity);
    }

    [Fact]
    public void Map_WhenOrderEntityIsNull_ShouldReturnNull()
    {
        //Act
        var order = _mapper.Map<Order>(null);

        //Assert
        Assert.Null(order);
    }

    [Fact]
    public void Map_WhenOrderIsNull_ShouldReturnNull()
    {
        //Act
        var orderEntity = _mapper.Map<OrderEntity>(null);

        //Assert
        Assert.Null(orderEntity);
    }
}
