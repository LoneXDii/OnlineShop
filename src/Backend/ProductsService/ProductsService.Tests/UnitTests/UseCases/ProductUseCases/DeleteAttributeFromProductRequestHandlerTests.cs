using System.Linq.Expressions;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.Exceptions;
using ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteAttributeFromProduct;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;

namespace ProductsService.Tests.UnitTests.UseCases.ProductUseCases;

public class DeleteAttributeFromProductRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<DeleteAttributeFromProductRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly DeleteAttributeFromProductRequestHandler _handler;

    public DeleteAttributeFromProductRequestHandlerTests()
    {
        _handler = new DeleteAttributeFromProductRequestHandler(
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProductAndAttributeExist_ShouldDeleteAttribute()
    {
        //Arrange
        var request = _fixture.Create<DeleteAttributeFromProductRequest>();

        var product = EntityFactory.CreateProduct();
        product.Categories.Add(new Category { Id = request.AttributeId });

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(product);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.DoesNotContain(product.Categories, c => c.Id == request.AttributeId);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProductDoesNotExist_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<DeleteAttributeFromProductRequest>();

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync((Product?)null);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such product or attribute of this product", exception.Message);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenAttributeDoesNotExist_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<DeleteAttributeFromProductRequest>();
        var product = EntityFactory.CreateProduct();
        product.Categories.Add(new Category { Id = request.AttributeId + 1 });

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(product);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such product or attribute of this product", exception.Message);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}