using AutoFixture;
using AutoMapper;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Mapping;
using OrderService.Infrastructure.Models;

namespace OrderService.Tests.UnitTests.MappingProfiles.Infrastructure;

public class ProducedOrderMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public ProducedOrderMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProducedOrderMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenOrderEntityIsNotNull_ShouldReturnCorrectProducedOrderDTO()
    {
        //Arrange
        var orderEntity = _fixture.Create<OrderEntity>();

        //Act
        var producedOrderDto = _mapper.Map<ProducedOrderDTO>(orderEntity);

        //Assert
        Assert.NotNull(producedOrderDto);
        Assert.Equal(orderEntity.Id, producedOrderDto.Id);
        Assert.Equal(orderEntity.OrderStatus, producedOrderDto.OrderStatus);
        Assert.Equal(orderEntity.UserId, producedOrderDto.UserId);
        Assert.Equal(orderEntity.TotalPrice, producedOrderDto.TotalPrice);
        Assert.Equal(orderEntity.Products, producedOrderDto.Products);
    }

    [Fact]
    public void Map_WhenOrderEntityIsNull_ShouldReturnNull()
    {
        //Act
        var producedOrderDto = _mapper.Map<ProducedOrderDTO>(null);

        //Assert
        Assert.Null(producedOrderDto);
    }
}
