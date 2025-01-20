using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Application.Mapping.Categories;
using ProductsService.Application.Mapping.Discounts;
using ProductsService.Application.Mapping.Products;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;

namespace ProductsService.Tests.UnitTests.MappingProfiles.Application.Products;

public class ProductDtoMappingProfileTests
{
    private readonly IMapper _mapper;

    public ProductDtoMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductDtoMappingProfile>();
            cfg.AddProfile<AttributeValueMappingProfile>();
            cfg.AddProfile<CategoryMappingProfile>();
            cfg.AddProfile<DiscountDtoMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenProductIsNotNull_ShouldMapToResponseProductDTO()
    {
        //Arrange
        var category = EntityFactory.CreateCategory();
        category.ParentId = null;
        var discount = EntityFactory.CreateDiscount();

        var attribute = EntityFactory.CreateCategory();
        attribute.ParentId = category.Id;
        attribute.Parent = category;
        category.Children.Add(attribute);
        
        var value = EntityFactory.CreateCategory();
        value.ParentId = attribute.Id;
        value.Parent = attribute;
        value.Children = null;
        attribute.Children.Add(value);
        
        var product = EntityFactory.CreateProduct();
        product.Categories.Add(category);
        product.Categories.Add(attribute);
        product.Categories.Add(value);
        product.Discount = discount;
        
        //Act
        var responseProductDto = _mapper.Map<ResponseProductDTO>(product);
        
        //Assert
        Assert.NotNull(responseProductDto);
        Assert.Equal(product.Id, responseProductDto.Id);
        Assert.Equal(product.Name, responseProductDto.Name);
        Assert.Equal(product.Price, responseProductDto.Price);
        Assert.Equal(product.Quantity, responseProductDto.Quantity);
        Assert.Equal(product.ImageUrl, responseProductDto.ImageUrl);
        Assert.NotNull(responseProductDto.Discount);
        Assert.Equal(discount.Id, responseProductDto.Discount.Id); 
        Assert.NotNull(responseProductDto.Category);
        Assert.Equal(category.Id, responseProductDto.Category.Id); 
        Assert.Single(responseProductDto.AttributeValues); 
    }

    [Fact] public void Map_WhenNoAttributesAndValuesInProduct_ShouldReturnEmptyAttributeValuesField()
    {
        //Arrange
        var category = EntityFactory.CreateCategory();
        category.ParentId = null;
        var discount = EntityFactory.CreateDiscount();

        var attribute = EntityFactory.CreateCategory();
        attribute.ParentId = category.Id;
        attribute.Parent = category;
        category.Children.Add(attribute);
        
        var product = EntityFactory.CreateProduct();
        product.Categories.Add(category);
        product.Categories.Add(attribute);
        product.Discount = discount;
        
        //Act
        var responseProductDto = _mapper.Map<ResponseProductDTO>(product);
        
        //Assert
        Assert.NotNull(responseProductDto);
        Assert.Empty(responseProductDto.AttributeValues); 
    }
    
    [Fact]
    public void Map_WhenDiscountIsNull_ShouldReturnWithNullDiscountField()
    {
        //Arrange
        var category = EntityFactory.CreateCategory();
        category.ParentId = null;
        var product = EntityFactory.CreateProduct();
        product.Categories.Add(category);
        
        //Act
        var responseProductDto = _mapper.Map<ResponseProductDTO>(product);
        
        //Assert
        Assert.NotNull(responseProductDto);
        Assert.Null(responseProductDto.Discount);
    }
    
    [Fact]
    public void Map_WhenProductIsNull_ShouldReturnNull()
    {
        //Arrange
        Product? product = null;
        
        //Act
        var responseProductDto = _mapper.Map<ResponseProductDTO>(product);
        
        //Assert
        Assert.Null(responseProductDto);
    }
}
