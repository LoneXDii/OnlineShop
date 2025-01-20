using System.Linq.Expressions;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.Exceptions;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductIfSufficientQuantity;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UseCases.ProductUseCases;

public class GetProductIfSufficientQuantityRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<GetProductIfSufficientQuantityRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly GetProductIfSufficientQuantityRequestHandler _handler;

    public GetProductIfSufficientQuantityRequestHandlerTests()
    {
        _handler = new GetProductIfSufficientQuantityRequestHandler(_unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProductExistsAndQuantityIsSufficient_ShouldReturnProduct()
    {
        //Arrange
        var request = _fixture.Create<GetProductIfSufficientQuantityRequest>();
        var product = new Product { Id = request.Id, Quantity = request.Quantity + 1 };

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ProductQueryRepository.GetByIdAsync(request.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(product);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(request.Quantity, result.Quantity);
    }

    [Fact]
    public async Task Handle_WhenProductDoesNotExist_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<GetProductIfSufficientQuantityRequest>();

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ProductQueryRepository.GetByIdAsync(request.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync((Product?)null);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such product", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenQuantityIsTooLow_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<GetProductIfSufficientQuantityRequest>();
        var product = new Product { Id = request.Id, Quantity = request.Quantity - 1 };

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ProductQueryRepository.GetByIdAsync(request.Id, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(product);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("Product's quantity is too low", exception.Message);
    }
}