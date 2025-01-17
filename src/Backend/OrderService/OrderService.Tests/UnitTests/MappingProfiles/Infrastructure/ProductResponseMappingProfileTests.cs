using AutoFixture;
using AutoMapper;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Mapping;
using OrderService.Infrastructure.Protos;

namespace OrderService.Tests.UnitTests.MappingProfiles.Infrastructure;

public class ProductResponseMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public ProductResponseMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductResponseMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenProductResponseIsNotNull_ShouldReturnCorrectProductEntity()
    {
        // Arrange
        var productResponse = _fixture.Create<ProductResponse>();

        // Act
        var productEntity = _mapper.Map<ProductEntity>(productResponse);

        // Assert
        Assert.NotNull(productEntity);
        Assert.Equal(productResponse.Id, productEntity.Id);
        Assert.Equal(productResponse.Name, productEntity.Name);
        Assert.Equal(productResponse.Quantity, productEntity.Quantity);
        Assert.Equal(productResponse.Price, productEntity.Price);
        Assert.Equal(productResponse.ImageUrl, productEntity.ImageUrl);
        Assert.Equal(productResponse.PriceId, productEntity.PriceId);
        Assert.Equal(productResponse.Discount, productEntity.Discount);
    }

    [Fact]
    public void Map_WhenProductResponseIsNull_ShouldReturnNull()
    {
        // Act
        var productEntity = _mapper.Map<ProductEntity>(null);

        // Assert
        Assert.Null(productEntity);
    }
}
