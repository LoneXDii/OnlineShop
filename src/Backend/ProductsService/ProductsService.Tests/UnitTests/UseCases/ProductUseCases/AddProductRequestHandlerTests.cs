using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.UseCases.ProductUseCases.Commands.AddProduct;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.MessageBrocker;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;
using ProductsService.Tests.Setups;

namespace ProductsService.Tests.UnitTests.UseCases.ProductUseCases;

public class AddProductRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBlobService> _blobServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IProducerService> _producerServiceMock = new();
    private readonly Mock<ILogger<AddProductRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly Mock<Stream> _streamMock = new();
    private readonly AddProductRequestHandler _handler;

    public AddProductRequestHandlerTests()
    {
        _unitOfWorkMock.SetupUnitOfWork();
        _blobServiceMock.SetupBlobService();

        _handler = new AddProductRequestHandler(
            _unitOfWorkMock.Object,
            _blobServiceMock.Object,
            _mapperMock.Object,
            _producerServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenImageIsProvided_ShouldUploadImageAndCreateProduct()
    {
        //Arrange
        var request = new AddProductRequest("", 1, 1, _streamMock.Object, "", []);
        var product = EntityFactory.CreateProduct();

        _mapperMock.Setup(mapper => mapper.Map<Product>(request)).Returns(product);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal("new-image-url", product.ImageUrl);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ProductCommandRepository.AddAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _blobServiceMock.Verify(service => service.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Once);
        _producerServiceMock.Verify(producer => producer.ProduceProductCreationAsync(product, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenImageIsNotProvided_ShouldCreateProductWithoutImage()
    {
        //Arrange
        var request = new AddProductRequest("", 1, 1, null, null, []);
        var product = EntityFactory.CreateProduct();

        _mapperMock.Setup(mapper => mapper.Map<Product>(request)).Returns(product);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Null(product.ImageUrl);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ProductCommandRepository.AddAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _blobServiceMock.Verify(service => service.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
        _producerServiceMock.Verify(producer => producer.ProduceProductCreationAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}