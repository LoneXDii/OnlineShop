using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddCategory;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;
using ProductsService.Tests.Setups;

namespace ProductsService.Tests.UnitTests.UseCases.CategoryUseCases;

public class AddCategoryRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IBlobService> _blobServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<AddCategoryRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly Mock<Stream> _streamMock = new();
    private readonly AddCategoryRequestHandler _handler;

    public AddCategoryRequestHandlerTests()
    {
        _unitOfWorkMock.SetupUnitOfWork();
        _blobServiceMock.SetupBlobService();

        _handler = new AddCategoryRequestHandler(
            _unitOfWorkMock.Object,
            _blobServiceMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenImageIsProvided_ShouldUploadImageAndAddCategory()
    {
        //Arrange
        var request = new AddCategoryRequest(_fixture.Create<string>(), _streamMock.Object, "image/png");

        var category = EntityFactory.CreateCategory();

        _mapperMock.Setup(mapper => mapper.Map<Category>(It.IsAny<AddCategoryRequest>()))
            .Returns(category);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal("new-image-url", category.ImageUrl);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.CategoryCommandRepository.AddAsync(category, It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);

        _blobServiceMock.Verify(service =>
            service.UploadAsync(request.Image, request.ImageContentType), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNoImageIsProvided_ShouldAddCategoryWithoutImageUrl()
    {
        //Arrange
        var request = new AddCategoryRequest(_fixture.Create<string>(), null, null);

        var category = EntityFactory.CreateCategory();

        _mapperMock.Setup(mapper => mapper.Map<Category>(It.IsAny<AddCategoryRequest>()))
            .Returns(category);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.CategoryCommandRepository.AddAsync(category, It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);

        _blobServiceMock.Verify(service =>
            service.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
    }
}