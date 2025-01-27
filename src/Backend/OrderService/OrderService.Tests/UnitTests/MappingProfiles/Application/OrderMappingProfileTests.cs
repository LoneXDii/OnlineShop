using AutoFixture;
using AutoMapper;
using OrderService.Application.DTO;
using OrderService.Application.Mapping;
using OrderService.Domain.Entities;

namespace OrderService.Tests.UnitTests.MappingProfiles.Application;

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
    public void Map_WhenOrderEntityIsNotNull_ShouldReturnCorrectOrderDTO()
    {
        //Arrange
        var orderEntity = _fixture.Create<OrderEntity>();

        //Act
        var orderDTO = _mapper.Map<OrderDTO>(orderEntity);

        //Assert
        Assert.NotNull(orderDTO);
        Assert.Equal(orderEntity.Id, orderDTO.Id);
        Assert.Equal(orderEntity.OrderStatus, orderDTO.OrderStatus);
        Assert.Equal(orderEntity.PaymentStatus, orderDTO.PaymentStatus);
        Assert.Equal(orderEntity.UserId, orderDTO.UserId);
        Assert.Equal(orderEntity.TotalPrice, orderDTO.TotalPrice);
        Assert.Equal(orderEntity.CreatedAt, orderDTO.CreatedAt);
        Assert.Equal(orderEntity.Products, orderDTO.Products);
    }

    [Fact]
    public void Map_WhenOrderEntityIsNull_ShouldReturnNull()
    {
        //Act
        var orderDTO = _mapper.Map<OrderDTO>(null);

        //Assert
        Assert.Null(orderDTO);
    }
}
