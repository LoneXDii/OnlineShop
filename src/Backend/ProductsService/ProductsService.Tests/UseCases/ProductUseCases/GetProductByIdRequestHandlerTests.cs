using System.Linq.Expressions;
using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.DTO;
using ProductsService.Application.Exceptions;
using ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductById;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;

namespace ProductsService.Tests.UseCases.ProductUseCases;

public class GetProductByIdRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<GetProductByIdRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly GetProductByIdRequestHandler _handler;

    public GetProductByIdRequestHandlerTests()
    {
        _handler = new GetProductByIdRequestHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProductExists_ShouldReturnProductDto()
    {
        //Arrange
        var request = _fixture.Create<GetProductByIdRequest>();
        var product = EntityFactory.CreateProduct();
        var productDto = _fixture.Create<ResponseProductDTO>();

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, object>>[]>()))
            .ReturnsAsync(product);

        _mapperMock.Setup(mapper => mapper.Map<ResponseProductDTO>(product)).Returns(productDto);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(productDto, result);
    }

    [Fact]
    public async Task Handle_WhenProductDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<GetProductByIdRequest>();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Product, object>>[]>()))
            .ReturnsAsync((Product?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such product", exception.Message);
    }
}