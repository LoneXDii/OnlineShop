using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.Exceptions;
using ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteProduct;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;
using ProductsService.Tests.Setups;

namespace ProductsService.Tests.UnitTests.UseCases.ProductUseCases;

public class DeleteProductRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBlobService> _blobServiceMock = new();
    private readonly Mock<ILogger<DeleteProductRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly DeleteProductRequestHandler _handler;

    public DeleteProductRequestHandlerTests()
    {
        _unitOfWorkMock.SetupUnitOfWork();
        _blobServiceMock.SetupBlobService();

        _handler = new DeleteProductRequestHandler(
            _unitOfWorkMock.Object,
            _blobServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProductExists_ShouldDeleteProductAndImage()
    {
        //Arrange
        var request = _fixture.Create<DeleteProductRequest>();
        var product = EntityFactory.CreateProduct("image-url");

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _blobServiceMock.Verify(service => service.DeleteAsync(product.ImageUrl), Times.Once);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ProductCommandRepository.DeleteAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProductDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<DeleteProductRequest>();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such product", exception.Message);
        _blobServiceMock.Verify(service => service.DeleteAsync(It.IsAny<string>()), Times.Never);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenProductHasNoImage_ShouldDeleteProductWithoutImage()
    {
        //Arrange
        var request = _fixture.Create<DeleteProductRequest>();
        var product = EntityFactory.CreateProduct();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _blobServiceMock.Verify(service => service.DeleteAsync(It.IsAny<string>()), Times.Never);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ProductCommandRepository.DeleteAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}