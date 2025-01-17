using AutoFixture;
using AutoMapper;
using OrderService.Application.DTO;
using OrderService.Application.Mapping;
using OrderService.Domain.Entities;

namespace OrderService.Tests.UnitTests.MappingProfiles.Application;

public class CartMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public CartMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CartMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenDictionaryIsNotEmpty_ShouldReturnCorrectCartDTO()
    {
        //Arrange
        var products = _fixture.CreateMany<ProductEntity>(3).ToDictionary(p => p.Id);
        foreach (var product in products.Values)
        {
            product.Quantity = _fixture.Create<int>() % 5 + 1; 
            product.Price = (double)(_fixture.Create<int>() % 10000)/100; 
            product.Discount = _fixture.Create<int>() % 50; 
        }

        //Act
        var cartDTO = _mapper.Map<CartDTO>(products);

        //Assert
        Assert.NotNull(cartDTO);
        Assert.Equal(products.Sum(item => item.Value.Quantity), cartDTO.Count);
        Assert.Equal(products.Sum(item => item.Value.Price * item.Value.Quantity * (100 - item.Value.Discount) / 100), cartDTO.TotalCost);
        Assert.Equal(products.Values.ToList(), cartDTO.Products);
    }

    [Fact]
    public void Map_WhenDictionaryIsEmpty_ShouldReturnEmptyCartDTO()
    {
        //Arrange
        var products = new Dictionary<int, ProductEntity>();

        //Act
        var cartDTO = _mapper.Map<CartDTO>(products);

        //Assert
        Assert.NotNull(cartDTO);
        Assert.Equal(0, cartDTO.Count);
        Assert.Equal(0, cartDTO.TotalCost);
        Assert.Empty(cartDTO.Products);
    }

    [Fact]
    public void Map_WhenDictionaryIsNull_ShouldReturnNull()
    {
        //Act
        var cartDTO = _mapper.Map<CartDTO>(null);

        //Assert
        Assert.Null(cartDTO);
    }
}
