using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.Exceptions;
using ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UseCases.ProductUseCases;

public class UpdateProductAttributeRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<UpdateProductAttributeRequestHandler>> _loggerMock = new();
    private readonly UpdateProductAttributeRequestHandler _handler;

    public UpdateProductAttributeRequestHandlerTests()
    {
        _handler = new UpdateProductAttributeRequestHandler(_unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenAttributesAreValid_ShouldUpdateProductAttribute()
    {
        //Arrange
        var request = new UpdateProductAttributeRequest { ProductId = 1, OldAttributeId = 2, NewAttributeId = 3 };
        var product = new Product { Id = request.ProductId, Categories = new List<Category> { new Category { Id = request.OldAttributeId, ParentId = 10 } } };
        var newAttribute = new Category { Id = request.NewAttributeId, ParentId = 10 };

        _unitOfWorkMock.Setup(unitOfWork => 
            unitOfWork.CategoryQueryRepository.GetByIdAsync(request.NewAttributeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(newAttribute);

        _unitOfWorkMock.Setup(unitOfWork => 
            unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(product);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.DoesNotContain(product.Categories, c => c.Id == request.OldAttributeId);
        Assert.Contains(product.Categories, c => c.Id == request.NewAttributeId);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenOldAttributeNotFound_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = new UpdateProductAttributeRequest { ProductId = 1, OldAttributeId = 2, NewAttributeId = 3 };
        var product = new Product { Id = request.ProductId, Categories = new List<Category>() };

        _unitOfWorkMock.Setup(unitOfWork => 
                unitOfWork.CategoryQueryRepository.GetByIdAsync(request.NewAttributeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category { Id = request.NewAttributeId, ParentId = 10 });

        _unitOfWorkMock.Setup(unitOfWork => 
                unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(product);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No entities with this isd", exception.Message);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenNewAttributeHasDifferentParent_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = new UpdateProductAttributeRequest { ProductId = 1, OldAttributeId = 2, NewAttributeId = 3 };
        var product = new Product { Id = request.ProductId, Categories = new List<Category> { new Category { Id = request.OldAttributeId, ParentId = 10 } } };
        var newAttribute = new Category { Id = request.NewAttributeId, ParentId = 20 };

        _unitOfWorkMock.Setup(unitOfWork => 
                unitOfWork.CategoryQueryRepository.GetByIdAsync(request.NewAttributeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(newAttribute);

        _unitOfWorkMock.Setup(unitOfWork => 
                unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(product);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("Attributes must have the same parent", exception.Message);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenNewAttributeAlreadyExists_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = new UpdateProductAttributeRequest { ProductId = 1, OldAttributeId = 2, NewAttributeId = 2 };
        var product = new Product { Id = request.ProductId, Categories = new List<Category> { new Category { Id = request.OldAttributeId, ParentId = 10} } };

        _unitOfWorkMock.Setup(unitOfWork => 
                unitOfWork.CategoryQueryRepository.GetByIdAsync(request.NewAttributeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category { Id = request.NewAttributeId, ParentId = 10 });

        _unitOfWorkMock.Setup(unitOfWork => 
                unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(product);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("Сan't add an existing attribute", exception.Message);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}