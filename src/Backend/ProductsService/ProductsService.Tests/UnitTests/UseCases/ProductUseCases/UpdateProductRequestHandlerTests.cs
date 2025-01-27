using System.Linq.Expressions;
using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.Exceptions;
using ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProduct;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.MessageBrocker;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Setups;

namespace ProductsService.Tests.UnitTests.UseCases.ProductUseCases;

public class UpdateProductRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IBlobService> _blobServiceMock = new();
    private readonly Mock<IProducerService> _producerServiceMock = new();
    private readonly Mock<ILogger<UpdateProductRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly Mock<Stream> _streamMock = new();
    private readonly UpdateProductRequestHandler _handler;

    public UpdateProductRequestHandlerTests()
    {
        _unitOfWorkMock.SetupUnitOfWork();
        _blobServiceMock.SetupBlobService();

        _handler = new UpdateProductRequestHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _blobServiceMock.Object,
            _producerServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProductExists_ShouldUpdateProduct()
    {
        //Arrange
        var request = new UpdateProductRequest { Image = _streamMock.Object, Price = 101 };
        var product = new Product { Id = request.Id, Price = 100, ImageUrl = "old-image-url" };

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.GetByIdAsync(request.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(product);

        _mapperMock.Setup(mapper => mapper.Map(request, product)).Callback(() => product.Price = request.Price.Value);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal("new-image-url", product.ImageUrl);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ProductCommandRepository.UpdateAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _blobServiceMock.Verify(blobService => blobService.DeleteAsync("old-image-url"), Times.Once);
        _blobServiceMock.Verify(blobService => blobService.UploadAsync(request.Image, request.ImageContentType), Times.Once);
        _producerServiceMock.Verify(producer => producer.ProduceProductCreationAsync(product, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProductDoesNotExist_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = new UpdateProductRequest();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.GetByIdAsync(request.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync((Product?)null);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such product", exception.Message);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ProductCommandRepository.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
        _blobServiceMock.Verify(blobService => blobService.DeleteAsync(It.IsAny<string>()), Times.Never);
        _blobServiceMock.Verify(blobService => blobService.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
        _producerServiceMock.Verify(producer => producer.ProduceProductCreationAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenImageIsNotProvided_ShouldUpdateProductWithoutImageChange()
    {
        //Arrange
        var request = new UpdateProductRequest { Image = null, Price = 101 };

        var product = new Product { Id = request.Id, ImageUrl = "existing-image-url", Price = 100 };

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.GetByIdAsync(request.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(product);

        _mapperMock.Setup(mapper => mapper.Map(request, product)).Callback(() => product.Price = request.Price.Value);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal("existing-image-url", product.ImageUrl);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ProductCommandRepository.UpdateAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _blobServiceMock.Verify(blobService => blobService.DeleteAsync(It.IsAny<string>()), Times.Never);
        _blobServiceMock.Verify(blobService => blobService.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
        _producerServiceMock.Verify(producer => producer.ProduceProductCreationAsync(product, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenPriseIsNotChanged_ShouldNotCallProducerService()
    {
        //Arrange
        var request = new UpdateProductRequest { Image = null, Price = 100 };

        var product = new Product { Id = request.Id, ImageUrl = "existing-image-url", Price = 100 };

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.GetByIdAsync(request.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(product);

        _mapperMock.Setup(mapper => mapper.Map(request, product)).Callback(() => product.Price = request.Price.Value);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _producerServiceMock.Verify(producer => producer.ProduceProductCreationAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}