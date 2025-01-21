using AutoMapper;
using ProductsService.API.Mapping;
using ProductsService.API.Protos;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;

namespace ProductsService.Tests.UnitTests.MappingProfiles.API;

public class ProductResponseMappingProfileTests
{
    private readonly IMapper _mapper;

    public ProductResponseMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductResponseMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenProductIsNotNull_ShouldMapToProductResponse()
    {
        //Arrange
        var product = EntityFactory.CreateProduct("imageUrl");
        var discount = new Discount
        {
            StartDate = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(1),
            Percent = 20
        };
        
        product.Discount = discount;

        //Act
        var productResponse = _mapper.Map<ProductResponse>(product);

        //Assert
        Assert.NotNull(productResponse);
        Assert.Equal(product.Id, productResponse.Id);
        Assert.Equal(product.Name, productResponse.Name);
        Assert.Equal(product.Price, productResponse.Price);
        Assert.Equal(product.ImageUrl, productResponse.ImageUrl);
        Assert.Equal(product.PriceId, productResponse.PriceId);
        Assert.Equal(product.Discount.Percent, productResponse.Dicsount);
    }

    [Fact]
    public void Map_WhenProductIsNull_ShouldReturnNull()
    {
        //Arrange
        Product? product = null;

        //Act
        var productResponse = _mapper.Map<ProductResponse>(product);

        //Assert
        Assert.Null(productResponse);
    }

    [Fact]
    public void Map_WhenProductHasNullImageUrl_ShouldSetImageUrlToEmptyString()
    {
        //Arrange
        var product = EntityFactory.CreateProduct();

        //Act
        var productResponse = _mapper.Map<ProductResponse>(product);

        //Assert
        Assert.NotNull(productResponse);
        Assert.Equal(string.Empty, productResponse.ImageUrl);
    }

    [Fact]
    public void Map_WhenProductHasNullPriceId_ShouldSetPriceIdToEmptyString()
    {
        //Arrange
        var product = EntityFactory.CreateProduct();
        product.PriceId = null;
        
        //Act
        var productResponse = _mapper.Map<ProductResponse>(product);

        //Assert
        Assert.NotNull(productResponse);
        Assert.Equal(string.Empty, productResponse.PriceId);
    }

    [Fact]
    public void Map_WhenDiscountIsNull_ShouldSetDiscountToZero()
    {
        //Arrange
        var product = EntityFactory.CreateProduct();

        //Act
        var productResponse = _mapper.Map<ProductResponse>(product);

        //Assert
        Assert.NotNull(productResponse);
        Assert.Equal(0, productResponse.Dicsount);
    }

    [Fact]
    public void Map_WhenDiscountIsActive_ShouldSetDiscountToCorrectPercent()
    {
        //Arrange
        var product = EntityFactory.CreateProduct();
        var discount = new Discount
        {
            StartDate = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(1),
            Percent = 20
        };
        
        product.Discount = discount;

        //Act
        var productResponse = _mapper.Map<ProductResponse>(product);

        //Assert
        Assert.NotNull(productResponse);
        Assert.Equal(discount.Percent, productResponse.Dicsount);
    }

    [Fact]
    public void Map_WhenDiscountIsInactive_ShouldSetDiscountToZero()
    {
        //Arrange
        var product = EntityFactory.CreateProduct();
        var discount = new Discount
        {
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(2),
            Percent = 30
        };
        
        product.Discount = discount;

        //Act
        var productResponse = _mapper.Map<ProductResponse>(product);

        //Assert
        Assert.NotNull(productResponse);
        Assert.Equal(0, productResponse.Dicsount);
    }
}