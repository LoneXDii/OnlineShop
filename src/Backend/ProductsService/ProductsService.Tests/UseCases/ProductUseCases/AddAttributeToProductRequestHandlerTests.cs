using System.Linq.Expressions;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.Exceptions;
using ProductsService.Application.UseCases.ProductUseCases.Commands.AddAttributeToProduct;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;

namespace ProductsService.Tests.UseCases.ProductUseCases;

public class AddAttributeToProductRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<AddAttributeToProductRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly AddAttributeToProductRequestHandler _handler;

    public AddAttributeToProductRequestHandlerTests()
    {
        _handler = new AddAttributeToProductRequestHandler(
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProductAndAttributeExist_ShouldAddAttributeToProduct()
    {
        //Arrange
        var request = _fixture.Create<AddAttributeToProductRequest>();
        var product = EntityFactory.CreateProduct();
        var attribute = EntityFactory.CreateCategory();
        product.Categories.Add(new Category() { Id = attribute.ParentId.Value });
        
        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(request.AttributeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(attribute);

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>(), 
                    It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(product);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Contains(attribute, product.Categories);
        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProductDoesNotExist_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<AddAttributeToProductRequest>();
        var attribute = EntityFactory.CreateCategory();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(request.AttributeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(attribute);

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>(), 
                    It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync((Product?)null);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
        
        //Assert
        Assert.Equal("No such product or attribute", exception.Message);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenAttributeDoesNotExist_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<AddAttributeToProductRequest>();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(request.AttributeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>(), 
                    It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(new Product());

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
        
        //Assert
        Assert.Equal("No such product or attribute", exception.Message);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenProductAlreadyHasAttribute_ShouldThrowBadRequestException()
    {
        // Arrange
        var request = _fixture.Create<AddAttributeToProductRequest>();
        var product = EntityFactory.CreateProduct();
        var attribute = EntityFactory.CreateCategory();

        product.Categories.Add(attribute);

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(request.AttributeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(attribute);

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>(), 
                    It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(product);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
        
        //Assert
        Assert.Equal("This product already has such an attribute", exception.Message);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenParentAttributeNotFound_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<AddAttributeToProductRequest>();
        var product = EntityFactory.CreateProduct();
        var attribute = EntityFactory.CreateCategory();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(request.AttributeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(attribute);

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>(), 
                    It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(product);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
        
        //Assert
        Assert.Equal("No parent for this attribute in this product", exception.Message);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}