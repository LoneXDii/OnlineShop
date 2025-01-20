using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.Exceptions;
using ProductsService.Application.Specifications.Products;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.DeleteCategory;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;
using ProductsService.Tests.Setups;

namespace ProductsService.Tests.UnitTests.UseCases.CategoryUseCases;

public class DeleteCategoryRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<DeleteCategoryRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly DeleteCategoryRequestHandler _handler;

    public DeleteCategoryRequestHandlerTests()
    {
        _unitOfWorkMock.SetupUnitOfWork();

        _handler = new DeleteCategoryRequestHandler(
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<DeleteCategoryRequest>();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(request.categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such category", exception.Message);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.CategoryCommandRepository.DeleteAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Never);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenCategoryIsInUse_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<DeleteCategoryRequest>();

        var category = EntityFactory.CreateCategory();

        var products = new List<Product>
        {
            EntityFactory.CreateProduct()
        };

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(request.categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.ListAsync(It.IsAny<ProductCategoryOrAtributeSpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("Can't delete category that is in use", exception.Message);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.CategoryCommandRepository.DeleteAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Never);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenCategoryIsNotInUse_ShouldDeleteCategory()
    {
        //Arrange
        var request = _fixture.Create<DeleteCategoryRequest>();

        var category = EntityFactory.CreateCategory();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(request.categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.ListAsync(It.IsAny<ProductCategoryOrAtributeSpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.CategoryCommandRepository.DeleteAsync(category, It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}