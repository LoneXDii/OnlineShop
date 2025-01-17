using AutoFixture;
using AutoMapper;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Mapping;
using OrderService.Infrastructure.Protos;

namespace OrderService.Tests.UnitTests.MappingProfiles.Infrastructure;

public class ProductRequestMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public ProductRequestMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductRequestMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenProductEntityIsNotNull_ShouldReturnCorrectProductRequest()
    {
        //Arrange
        var productEntity = _fixture.Create<ProductEntity>();

        //Act
        var productRequest = _mapper.Map<ProductRequest>(productEntity);

        //Assert
        Assert.NotNull(productRequest);
        Assert.Equal(productEntity.Id, productRequest.Id);
        Assert.Equal(productEntity.Quantity, productRequest.Quantity);
    }

    [Fact]
    public void Map_WhenProductEntityIsNull_ShouldReturnNull()
    {
        //Act
        var productRequest = _mapper.Map<ProductRequest>(null);

        //Assert
        Assert.Null(productRequest);
    }
}
