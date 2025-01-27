using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using ProductsService.Application.DTO;
using ProductsService.Application.Mapping.Common;
using ProductsService.Application.Mapping.Products;
using ProductsService.Application.UseCases.ProductUseCases.Commands.AddProduct;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UnitTests.MappingProfiles.Application.Products;

public class AddProductRequestMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();
    private readonly Mock<IFormFile> _formFileMock = new();
    private readonly Mock<Stream> _streamMock = new();
    
    public AddProductRequestMappingProfileTests()
    {
        _formFileMock.Setup(f => f.OpenReadStream()).Returns(_streamMock.Object);
        _formFileMock.Setup(f => f.ContentType).Returns("image/jpeg");
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AddProductRequestMappingProfile>();
            cfg.AddProfile<FormFileToStreamMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenAddProductDTOIsNotNull_ShouldMapToAddProductRequest()
    {
        //Arrange
        var addProductDto = _fixture.Build<AddProductDTO>()
            .With(dto => dto.Image, _formFileMock.Object)
            .Create();
        
        //Act
        var addProductRequest = _mapper.Map<AddProductRequest>(addProductDto);
        
        //Assert
        Assert.NotNull(addProductRequest);
        Assert.Equal(addProductDto.Name, addProductRequest.Name);
        Assert.Equal(addProductDto.Price, addProductRequest.Price);
        Assert.Equal(addProductDto.Quantity, addProductRequest.Quantity);
        Assert.NotNull(addProductRequest.Image);
        Assert.Equal(_formFileMock.Object.ContentType, addProductRequest.ImageContentType);
        Assert.Equal(addProductDto.Attributes, addProductRequest.Attributes);
    }

    [Fact]
    public void Map_WhenAddProductRequestIsNotNull_ShouldMapToProduct()
    {
        //Arrange
        var addProductRequest = new AddProductRequest("Test Product", 19.99, 10, _streamMock.Object, 
            "image/jpeg", [1, 2, 3]);
        
        //Act
        var product = _mapper.Map<Product>(addProductRequest);
        
        //Assert
        Assert.NotNull(product);
        Assert.Equal(addProductRequest.Name, product.Name);
        Assert.Equal(addProductRequest.Price, product.Price);
        Assert.Equal(addProductRequest.Quantity, product.Quantity);
        Assert.Equal(addProductRequest.Attributes.Length, product.Categories.Count);
        Assert.All(product.Categories, category => Assert.Contains(addProductRequest.Attributes, attr => attr == category.Id));
    }

    [Fact]
    public void Map_WhenAddProductDTOIsNull_ShouldReturnNull()
    {
        //Arrange
        AddProductDTO? addProductDto = null;
        
        //Act
        var addProductRequest = _mapper.Map<AddProductRequest>(addProductDto);
        
        //Assert
        Assert.Null(addProductRequest);
    }

    [Fact]
    public void Map_WhenAddProductRequestIsNull_ShouldReturnNull()
    {
        //Arrange
        AddProductRequest? addProductRequest = null;
        
        //Act
        var product = _mapper.Map<Product>(addProductRequest);
        
        //Assert
        Assert.Null(product);
    }
}
