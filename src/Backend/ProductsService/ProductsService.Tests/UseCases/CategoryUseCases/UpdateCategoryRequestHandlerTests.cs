using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.Exceptions;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.UpdateCategory;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UseCases.CategoryUseCases;

public class UpdateCategoryRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBlobService> _blobServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<UpdateCategoryRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly Mock<Stream> _streamMock = new();
    private readonly UpdateCategoryRequestHandler _handler;

    public UpdateCategoryRequestHandlerTests()
    {
        _handler = new UpdateCategoryRequestHandler(
            _unitOfWorkMock.Object,
            _blobServiceMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoryExistsAndImageIsProvided_ShouldUpdateCategoryAndUploadImage()
    {
        //Arrange
        var categoryId = _fixture.Create<int>();
        var request = new UpdateCategoryRequest
        {
            CategoryId = categoryId,
            Image = _streamMock.Object,
            ImageContentType = "image/jpeg",
        };

        var category = _fixture.Build<Category>()
            .With(c => c.ImageUrl, (string?)null)
            .Without(c => c.Parent)
            .Without(c => c.Products)
            .Without(c => c.Children)
            .Create();
        
        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        
        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryCommandRepository.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mapperMock.Setup(mapper => mapper.Map(It.IsAny<UpdateCategoryRequest>(), category))
            .Verifiable();

        _blobServiceMock.Setup(blobService =>
                blobService.UploadAsync(request.Image, request.ImageContentType))
            .ReturnsAsync("new-image-url");

        //Act
        await _handler.Handle(request, default);

        //Assert
        Assert.Equal("new-image-url", category.ImageUrl);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.CategoryCommandRepository.UpdateAsync(category, It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        
        _blobServiceMock.Verify(service => 
            service.UploadAsync(request.Image, request.ImageContentType), Times.Once);
        
        _blobServiceMock.Verify(service => 
            service.DeleteAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenCategoryExistsAndImageIsProvidedAndPreviousImageExists_ShouldDeletePreviousImage()
    {
        //Arrange
        var categoryId = _fixture.Create<int>();
        var request = new UpdateCategoryRequest
        {
            CategoryId = categoryId,
            Image = _streamMock.Object,
            ImageContentType = "image/jpeg",
        };

        var category = _fixture.Build<Category>()
            .With(c => c.ImageUrl, "old-image-url")
            .Without(c => c.Parent)
            .Without(c => c.Products)
            .Without(c => c.Children)
            .Create();
        
        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryCommandRepository.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        _mapperMock.Setup(mapper => mapper.Map(It.IsAny<UpdateCategoryRequest>(), category))
            .Verifiable();

        _blobServiceMock.Setup(blobService =>
                blobService.UploadAsync(request.Image, request.ImageContentType))
            .ReturnsAsync("new-image-url");
        
        _blobServiceMock.Setup(blobService =>
                blobService.DeleteAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        //Act
        await _handler.Handle(request, default);

        //Assert
        Assert.Equal("new-image-url", category.ImageUrl);

        _blobServiceMock.Verify(service => 
            service.DeleteAsync("old-image-url"), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ShouldThrowBadRequestException()
    {
        // Arrange
        var categoryId = _fixture.Create<int>();
        var request = new UpdateCategoryRequest
        {
            CategoryId = categoryId,
            Image = null,
            ImageContentType = null,
        };

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        //Act
        await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, default));

        //Assert
        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.CategoryCommandRepository.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Never);
        
        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
        
        _blobServiceMock.Verify(service => 
            service.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
        
        _blobServiceMock.Verify(service => 
            service.DeleteAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenNoImageIsProvided_ShouldUpdateCategoryWithoutImageUrl()
    {
        //Arrange
        var categoryId = _fixture.Create<int>();
        var request = new UpdateCategoryRequest
        {
            CategoryId = categoryId,
            Image = null,
            ImageContentType = null,
        };

        var category = _fixture.Build<Category>()
            .Without(c => c.Parent)
            .Without(c => c.Products)
            .Without(c => c.Children)
            .Create();
        
        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryCommandRepository.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        _mapperMock.Setup(mapper => mapper.Map(It.IsAny<UpdateCategoryRequest>(), category))
            .Verifiable();

        //Act
        await _handler.Handle(request, default);

        //Assert
        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.CategoryCommandRepository.UpdateAsync(category, It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        
        _blobServiceMock.Verify(service => 
            service.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
        
        _blobServiceMock.Verify(service => 
            service.DeleteAsync(It.IsAny<string>()), Times.Never);
    }
}