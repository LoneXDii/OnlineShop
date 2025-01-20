using AutoMapper;
using ProductsService.Domain.Entities;
using ProductsService.Infrastructure.Mapping;
using ProductsService.Infrastructure.Models;
using ProductsService.Tests.Factories;

namespace ProductsService.Tests.UnitTests.MappingProfiles.Infrastructure;

public class ProductCreationDtoMappingProfileTests
{
    private readonly IMapper _mapper;

    public ProductCreationDtoMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductCreationDtoMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenProductIsNotNull_ShouldMapToProductCreationDto()
    {
        //Arrange
        var product = EntityFactory.CreateProduct();
        
        //Act
        var productDto = _mapper.Map<ProductCreationDTO>(product);
        
        //Assert
        Assert.NotNull(productDto);
        Assert.Equal(product.Id, productDto.Id);
        Assert.Equal(product.Name, productDto.Name);
        Assert.Equal(product.Price, productDto.Price);
    }
    
    [Fact]
    public void Map_WhenProductIsNull_ShouldReturnNull()
    {
        //Arrange
        Product? product = null;
        
        //Act
        var productDto = _mapper.Map<ProductCreationDTO>(product);
        
        //Assert
        Assert.Null(productDto);
    }
}