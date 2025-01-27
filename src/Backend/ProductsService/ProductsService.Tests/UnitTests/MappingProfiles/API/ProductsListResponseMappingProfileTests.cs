using AutoFixture;
using AutoMapper;
using ProductsService.API.Mapping;
using ProductsService.API.Protos;

namespace ProductsService.Tests.UnitTests.MappingProfiles.API;

public class ProductsListResponseMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();
    
    public ProductsListResponseMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductsListResponseMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenProductResponseListIsNotEmpty_ShouldMapToProductsListResponse()
    {
        //Arrange
        var productResponses = new List<ProductResponse>
        {
            _fixture.Create<ProductResponse>(),
            _fixture.Create<ProductResponse>()
        };

        //Act
        var productsListResponse = _mapper.Map<ProductsListResponse>(productResponses);

        //Assert
        Assert.NotNull(productsListResponse);
        Assert.NotNull(productsListResponse.Products);
        Assert.Equal(2, productsListResponse.Products.Count);
        Assert.Equal(productResponses[0].Id, productsListResponse.Products[0].Id);
        Assert.Equal(productResponses[1].Id, productsListResponse.Products[1].Id);
    }

    [Fact]
    public void Map_WhenProductResponseListIsEmpty_ShouldMapToProductsListResponseWithEmptyProducts()
    {
        //Arrange
        var productResponses = new List<ProductResponse>();

        //Act
        var productsListResponse = _mapper.Map<ProductsListResponse>(productResponses);

        //Assert
        Assert.NotNull(productsListResponse);
        Assert.NotNull(productsListResponse.Products);
        Assert.Empty(productsListResponse.Products);
    }

    [Fact]
    public void Map_WhenProductResponseListIsNull_ShouldReturnNull()
    {
        //Arrange
        List<ProductResponse>? productResponses = null;

        //Act
        var productsListResponse = _mapper.Map<ProductsListResponse>(productResponses);

        //Assert
        Assert.Null(productsListResponse);
    }
}