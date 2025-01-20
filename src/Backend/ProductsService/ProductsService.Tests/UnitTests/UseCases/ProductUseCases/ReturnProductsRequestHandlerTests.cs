using Moq;
using ProductsService.Application.Specifications.Products;
using ProductsService.Application.UseCases.ProductUseCases.Commands.ReturnProducts;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Setups;

namespace ProductsService.Tests.UnitTests.UseCases.ProductUseCases;

public class ReturnProductsRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly ReturnProductsRequestHandler _handler;

    public ReturnProductsRequestHandlerTests()
    {
        _unitOfWorkMock.SetupUnitOfWork();

        _handler = new ReturnProductsRequestHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProductsExist_ShouldUpdateQuantities()
    {
        //Arrange
        var request = new ReturnProductsRequest(new Dictionary<int, int>
        {
            { 1, 5 },
            { 2, 3 }
        });

        var products = new List<Product>
        {
            new Product { Id = 1, Quantity = 10 },
            new Product { Id = 2, Quantity = 20 }
        };

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.ListAsync(It.IsAny<ProductsListBuIdsSpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal(15, products[0].Quantity);
        Assert.Equal(23, products[1].Quantity);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ProductCommandRepository.UpdateAsync(products[0], It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ProductCommandRepository.UpdateAsync(products[1], It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNoProductsFound_ShouldNotUpdateAnything()
    {
        //Arrange
        var request = new ReturnProductsRequest(new Dictionary<int, int>
        {
            { 1, 5 }
        });

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.ListAsync(It.IsAny<ProductsListBuIdsSpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.ProductCommandRepository.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}