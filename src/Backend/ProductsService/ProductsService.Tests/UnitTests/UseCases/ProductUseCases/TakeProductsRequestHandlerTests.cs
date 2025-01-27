using System.Linq.Expressions;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.Exceptions;
using ProductsService.Application.UseCases.ProductUseCases.Queries.TakeProducts;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Setups;

namespace ProductsService.Tests.UnitTests.UseCases.ProductUseCases;

public class TakeProductsRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<TakeProductsRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly TakeProductsRequestHandler _handler;

    public TakeProductsRequestHandlerTests()
    {
        _unitOfWorkMock.SetupUnitOfWork();
        
        _handler = new TakeProductsRequestHandler(_unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProductsAreAvailable_ShouldUpdateQuantities()
    {
        //Arrange
        var request = _fixture.Build<TakeProductsRequest>()
            .With(req => req.Products, new Dictionary<int, int>{ { 1, 3 }, { 2, 2 } })
            .Create();

        var products = new List<Product>
        {
            new Product { Id = 1, Quantity = 5 },
            new Product { Id = 2, Quantity = 5 }
        };

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ProductQueryRepository.ListAsync(
                It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(products);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(3, products[0].Quantity);
        Assert.Equal(2, products[1].Quantity); 
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ProductCommandRepository.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProductQuantityIsTooLow_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Build<TakeProductsRequest>()
            .With(req => req.Products, new Dictionary<int, int>{ { 1, 10 } })
            .Create();
        
        var products = new List<Product>
        {
            new Product { Id = 1, Quantity = 5 } 
        };

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ProductQueryRepository.ListAsync(
                It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(products);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Contains("Quantity of product with id=1 is too low", exception.Message);
    }
}