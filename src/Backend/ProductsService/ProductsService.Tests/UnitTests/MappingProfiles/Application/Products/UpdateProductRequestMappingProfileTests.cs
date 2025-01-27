using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using ProductsService.Application.DTO;
using ProductsService.Application.Mapping.Common;
using ProductsService.Application.Mapping.Products;
using ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProduct;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;

namespace ProductsService.Tests.UnitTests.MappingProfiles.Application.Products;

public class UpdateProductRequestMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IFormFile> _formFileMock = new();
    private readonly Mock<Stream> _streamMock = new();
    private readonly Fixture _fixture = new();
    
    public UpdateProductRequestMappingProfileTests()
    {
        _formFileMock.Setup(f => f.OpenReadStream()).Returns(_streamMock.Object);
        _formFileMock.Setup(f => f.ContentType).Returns("image/jpeg");
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UpdateProductRequestMappingProfile>();
            cfg.AddProfile<FormFileToStreamMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenUpdateProductDTOIsNotNull_ShouldMapToUpdateProductRequest()
    {
        var updateProductDto = _fixture.Build<UpdateProductDTO>()
            .With(dto => dto.Image, _formFileMock.Object)
            .Create();

        //Act
        var updateProductRequest = _mapper.Map<UpdateProductRequest>(updateProductDto);

        //Assert
        Assert.NotNull(updateProductRequest);
        Assert.Equal(updateProductDto.Name, updateProductRequest.Name);
        Assert.Equal(updateProductDto.Price, updateProductRequest.Price);
        Assert.Equal(updateProductDto.Quantity, updateProductRequest.Quantity);
        Assert.NotNull(updateProductRequest.Image);
        Assert.Equal(_formFileMock.Object.ContentType, updateProductRequest.ImageContentType);
        Assert.Equal(updateProductDto.Attributes, updateProductRequest.Attributes);
    }

    [Fact]
    public void Map_WhenAllFieldsAreNull_ShouldNotToChangeObject()
    {
        //Arrange
        var existingProduct = EntityFactory.CreateProduct();

        var updateProductRequest = new UpdateProductRequest
        {
            Id = existingProduct.Id,
            Name = null, 
            Price = null, 
            Quantity = null 
        };

        //Act
        var product = _mapper.Map(updateProductRequest, existingProduct);

        //Assert
        Assert.NotNull(product);
        Assert.Equal(existingProduct.Id, product.Id);
        Assert.Equal(existingProduct.Name, product.Name); 
        Assert.Equal(existingProduct.Price, product.Price); 
        Assert.Equal(existingProduct.Quantity, product.Quantity); 
    }

    [Fact]
    public void Map_WhenNameIsNotNull_ShouldChangeName()
    {
        //Arrange
        var existingProduct = EntityFactory.CreateProduct();

        var updateProductRequest = new UpdateProductRequest
        {
            Id = existingProduct.Id,
            Name = "Changed", 
            Price = null, 
            Quantity = null 
        };

        var startName = existingProduct.Name;
        
        //Act
        var product = _mapper.Map(updateProductRequest, existingProduct);

        //Assert
        Assert.NotNull(product);
        Assert.Equal(existingProduct.Id, product.Id);
        Assert.Equal(updateProductRequest.Name, product.Name); 
        Assert.Equal(existingProduct.Price, product.Price); 
        Assert.Equal(existingProduct.Quantity, product.Quantity); 
        Assert.NotEqual(startName, product.Name);
    }
    
    [Fact]
    public void Map_WhenPriceIsNotNull_ShouldChangePrice()
    {
        //Arrange
        var existingProduct = EntityFactory.CreateProduct();

        var updateProductRequest = new UpdateProductRequest
        {
            Id = existingProduct.Id,
            Name = null, 
            Price = 123, 
            Quantity = null 
        };

        var startPrice = existingProduct.Price;
        
        //Act
        var product = _mapper.Map(updateProductRequest, existingProduct);

        //Assert
        Assert.NotNull(product);
        Assert.Equal(existingProduct.Id, product.Id);
        Assert.Equal(existingProduct.Name, product.Name); 
        Assert.Equal(existingProduct.Price, product.Price); 
        Assert.Equal(existingProduct.Quantity, product.Quantity); 
        Assert.NotEqual(startPrice, product.Price);
    }
    
    [Fact]
    public void Map_WhenQuantityIsNotNull_ShouldChangeQuantity()
    {
        //Arrange
        var existingProduct = EntityFactory.CreateProduct();

        var updateProductRequest = new UpdateProductRequest
        {
            Id = existingProduct.Id,
            Name = null, 
            Price = null, 
            Quantity = 123 
        };

        var startQuantity = existingProduct.Quantity;
        
        //Act
        var product = _mapper.Map(updateProductRequest, existingProduct);

        //Assert
        Assert.NotNull(product);
        Assert.Equal(existingProduct.Id, product.Id);
        Assert.Equal(existingProduct.Name, product.Name); 
        Assert.Equal(existingProduct.Price, product.Price); 
        Assert.Equal(existingProduct.Quantity, product.Quantity); 
        Assert.NotEqual(startQuantity, product.Quantity);
    }
    
    [Fact]
    public void Map_WhenUpdateProductDTOIsNull_ShouldReturnNull()
    {
        //Arrange
        UpdateProductDTO? updateProductDto = null;

        //Act
        var updateProductRequest = _mapper.Map<UpdateProductRequest>(updateProductDto);

        //Assert
        Assert.Null(updateProductRequest);
    }

    [Fact]
    public void Map_WhenUpdateProductRequestIsNull_ShouldReturnNull()
    {
        //Arrange
        UpdateProductRequest? updateProductRequest = null;

        //Act
        var product = _mapper.Map<Product>(updateProductRequest);

        //Assert
        Assert.Null(product);
    }
}