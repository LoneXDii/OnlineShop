using AutoFixture;
using AutoMapper;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Protos;
using OrderService.Infrastructure.Services;

namespace OrderService.Tests.UnitTests.Services;

public class GrpcProductServiceTests
{
    private readonly Mock<Products.ProductsClient> _productsClientMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<GrpcProductService>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly GrpcProductService _productGrpcService;

    public GrpcProductServiceTests()
    {
        _productGrpcService = new GrpcProductService(_productsClientMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetByIdIfSufficientQuantityAsync_WhenCalled_ShouldReturnProduct()
    {
        //Arrange
        var productResponse = _fixture.Create<ProductResponse>();
        var expectedResult = _fixture.Create<ProductEntity>();
        
        _productsClientMock.Setup(productsClient => productsClient.GetProductAsync(It.IsAny<ProductRequest>(), null,
                null, It.IsAny<CancellationToken>()))
            .Returns(new AsyncUnaryCall<ProductResponse>(Task.FromResult(productResponse), null, null, null, null));
        
        _mapperMock.Setup(mapper => mapper.Map<ProductEntity>(productResponse))
            .Returns(expectedResult);
        
        //Act
        var result = await _productGrpcService.GetByIdIfSufficientQuantityAsync(1, 1);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult, result);
        
        _productsClientMock.Verify(productsClient => productsClient.GetProductAsync(It.IsAny<ProductRequest>(), null,
            null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task TakeProductsIfSufficientQuantityAsync_WhenCalled_ShouldReturnProductsList()
    {
        //Arrange
        var productsListResponse = _fixture.Create<ProductsListResponse>();
        var expectedResult = _fixture.Create<List<ProductEntity>>();
        
        _productsClientMock.Setup(productsClient => productsClient.TakeProductsAsync(It.IsAny<ProductsListRequest>(), null,
                null, It.IsAny<CancellationToken>()))
            .Returns(new AsyncUnaryCall<ProductsListResponse>(Task.FromResult(productsListResponse), null, null, null, null));
        
        _mapperMock.Setup(mapper => mapper.Map<List<ProductRequest>>(It.IsAny<IEnumerable<ProductEntity>>()))
            .Returns([]);
        
        _mapperMock.Setup(mapper => mapper.Map<List<ProductEntity>>(It.IsAny<RepeatedField<ProductResponse>>()))
            .Returns(expectedResult);
        
        //Act
        var result = await _productGrpcService.TakeProductsIfSufficientQuantityAsync(new List<ProductEntity>());
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult, result);
        
        _productsClientMock.Verify(productsClient => productsClient.TakeProductsAsync(It.IsAny<ProductsListRequest>(), null,
            null, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task ReturnProductsAsync_WhenCalled_ShouldCallReturnProductsFunctionOnGrpcServer()
    {
        //Arrange
        _productsClientMock.Setup(productsClient => productsClient.ReturnProductsAsync(It.IsAny<ProductsListRequest>(), null,
                null, It.IsAny<CancellationToken>()))
            .Returns(_fixture.Create<AsyncUnaryCall<Empty>>());
        
        _mapperMock.Setup(mapper => mapper.Map<List<ProductRequest>>(It.IsAny<IEnumerable<ProductEntity>>()))
            .Returns([]);
        
        //Act
        await _productGrpcService.ReturnProductsAsync(new List<ProductEntity>());
        
        //Assert
        _productsClientMock.Verify(productsClient => productsClient.ReturnProductsAsync(It.IsAny<ProductsListRequest>(), null,
            null, It.IsAny<CancellationToken>()), Times.Once);
    }
}