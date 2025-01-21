using AutoFixture;
using AutoMapper;
using MediatR;
using Moq;
using ProductsService.API.Controllers;
using ProductsService.Application.DTO;
using ProductsService.Application.UseCases.ProductUseCases.Commands.AddAttributeToProduct;
using ProductsService.Application.UseCases.ProductUseCases.Commands.AddProduct;
using ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteAttributeFromProduct;
using ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteProduct;
using ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProduct;
using ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductById;
using ProductsService.Application.UseCases.ProductUseCases.Queries.ListProducts;

namespace ProductsService.Tests.UnitTests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ProductsController _controller;
    private readonly Fixture _fixture = new();
    
    public ProductsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _controller = new ProductsController(_mediatorMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetProducts_WhenCalled_ShouldCallListProductsWithPaginationRequest()
    {
        //Arrange
        var request = _fixture.Create<ListProductsWithPaginationRequest>();

        //Act
        await _controller.GetProducts(request, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetProductById_WhenCalled_ShouldCallGetProductByIdRequest()
    {
        //Arrange
        var request = _fixture.Create<GetProductByIdRequest>();

        //Act
        await _controller.GetProductById(request, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CreateProduct_WhenCalled_ShouldCallAddProductRequest()
    {
        //Arrange
        var product = new AddProductDTO();
        var request = new AddProductRequest("", 1, 1, null, null, []);

        _mapperMock.Setup(m => m.Map<AddProductRequest>(product)).Returns(request);

        //Act
        await _controller.CreateProduct(product, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task UpdateProduct_WhenCalled_ShouldCallUpdateProductRequest()
    {
        //Arrange
        var productId = 1;
        var product = new UpdateProductDTO();
        var request = new UpdateProductRequest();

        _mapperMock.Setup(m => m.Map<UpdateProductRequest>(product)).Returns(request);

        //Act
        await _controller.UpdateProduct(productId, product, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task DeleteProduct_WhenCalled_ShouldCallDeleteProductRequest()
    {
        //Arrange
        var request = _fixture.Create<DeleteProductRequest>();

        //Act
        await _controller.DeleteProduct(request, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task AddProductAttribute_WhenCalled_ShouldCallAddAttributeToProductRequest()
    {
        //Arrange
        var request = _fixture.Create<AddAttributeToProductRequest>();

        //Act
        await _controller.AddProductAttribute(request, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task UpdateProductAttribute_WhenCalled_ShouldCallUpdateProductAttributeRequest()
    {
        //Arrange
        var productAttribute = new RequestAttributeValueDTO();
        var newAttribute = new UpdateAttributeDTO { AttributeId = 2 };
        var request = new UpdateProductAttributeRequest();

        _mapperMock.Setup(m => m.Map<UpdateProductAttributeRequest>(productAttribute)).Returns(request);

        //Act
        await _controller.UpdateProductAttribute(productAttribute, newAttribute, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAttribute_WhenCalled_ShouldCallDeleteAttributeFromProductRequest()
    {
        //Arrange
        var request = _fixture.Create<DeleteAttributeFromProductRequest>();

        //Act
        await _controller.DeleteProductAttribute(request, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }
}