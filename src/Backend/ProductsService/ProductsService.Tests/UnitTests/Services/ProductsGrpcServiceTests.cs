using AutoFixture;
using AutoMapper;
using MediatR;
using Moq;
using ProductsService.API.Protos;
using ProductsService.API.Services;
using ProductsService.Application.UseCases.ProductUseCases.Commands.ReturnProducts;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductIfSufficientQuantity;
using ProductsService.Application.UseCases.ProductUseCases.Queries.TakeProducts;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;

namespace ProductsService.Tests.UnitTests.Services;

public class ProductsGrpcServiceTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly ProductsGrpcService _service;
    private readonly Fixture _fixture = new();
    
    public ProductsGrpcServiceTests()
    {
        _service = new ProductsGrpcService(_mediatorMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetProduct_ShouldReturnMappedProductResponse()
    {
        // Arrange
        var request = _fixture.Create<ProductRequest>();
        var product = EntityFactory.CreateProduct();
        var expectedResponse = _fixture.Create<ProductResponse>();

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductIfSufficientQuantityRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        _mapperMock.Setup(m => m.Map<ProductResponse>(product)).Returns(expectedResponse);

        // Act
        var response = await _service.GetProduct(request, null);

        // Assert
        Assert.Equal(expectedResponse, response);
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetProductIfSufficientQuantityRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<ProductResponse>(product), Times.Once);
    }

    [Fact]
    public async Task TakeProducts_ShouldReturnMappedProductsListResponse()
    {
        //Arrange
        var request = _fixture.Create<ProductsListRequest>();
        var productList = new List<Product> { EntityFactory.CreateProduct() };
        var expectedResponse = _fixture.Create<ProductsListResponse>();

        _mediatorMock.Setup(m => m.Send(It.IsAny<TakeProductsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(productList);
        _mapperMock.Setup(m => m.Map<List<ProductResponse>>(productList)).Returns(new List<ProductResponse> { new ProductResponse { Id = 1, Name = "Test Product" } });
        _mapperMock.Setup(m => m.Map<ProductsListResponse>(It.IsAny<List<ProductResponse>>())).Returns(expectedResponse);

        //Act
        var response = await _service.TakeProducts(request, null);

        //Assert
        Assert.Equal(expectedResponse, response);
        _mediatorMock.Verify(m => m.Send(It.IsAny<TakeProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<List<ProductResponse>>(productList), Times.Once);
        _mapperMock.Verify(m => m.Map<ProductsListResponse>(It.IsAny<List<ProductResponse>>()), Times.Once);
    }

    [Fact]
    public async Task ReturnProducts_ShouldSendReturnProductsRequest()
    {
        //Arrange
        var request = _fixture.Create<ProductsListRequest>();

        //Act
        await _service.ReturnProducts(request, null);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<ReturnProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

