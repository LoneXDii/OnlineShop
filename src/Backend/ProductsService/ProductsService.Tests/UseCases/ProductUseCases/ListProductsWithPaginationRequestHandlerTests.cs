using System.Linq.Expressions;
using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using ProductsService.Application.Configuration;
using ProductsService.Application.DTO;
using ProductsService.Application.Exceptions;
using ProductsService.Application.UseCases.ProductUseCases.Queries.ListProducts;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UseCases.ProductUseCases;

public class ListProductsWithPaginationRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IOptions<PaginationSettings>> _paginationOptionsMock = new();
    private readonly Fixture _fixture = new();
    private readonly ListProductsWithPaginationRequestHandler _handler;

    public ListProductsWithPaginationRequestHandlerTests()
    {
        var paginationSettings = new PaginationSettings { MaxPageSize = 100 };
        _paginationOptionsMock.Setup(opt => opt.Value).Returns(paginationSettings);

        _handler = new ListProductsWithPaginationRequestHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _paginationOptionsMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProductsExist_ShouldReturnPaginatedList()
    {
        //Arrange
        var request = _fixture.Build<ListProductsWithPaginationRequest>()
            .With(req => req.PageNo, 1)
            .With(req => req.PageSize, 10)
            .Create();

        var products = new List<Product>();
        
        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.ListWithPaginationAsync(
                    request.PageNo, request.PageSize,
                    It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Product, object>>>(),
                    It.IsAny<Expression<Func<Product, object>>>()))
            .ReturnsAsync(products);

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.ProductQueryRepository.CountAsync(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);

        var productsDto = _fixture.CreateMany<ResponseProductDTO>(2).ToList();
        _mapperMock.Setup(mapper => mapper.Map<List<ResponseProductDTO>>(products)).Returns(productsDto);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(request.PageNo, result.CurrentPage);
        Assert.Equal(1, result.TotalPages); 
        Assert.Equal(productsDto, result.Items);
    }

    [Fact]
    public async Task Handle_WhenPageNoIsGreaterThanTotalPages_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Build<ListProductsWithPaginationRequest>()
            .With(req => req.PageNo, 2)
            .With(req => req.PageSize, 10)
            .Create();

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ProductQueryRepository.CountAsync(It.IsAny<ISpecification<Product>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(5);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        
        //Assert
        Assert.Equal("No such page", exception.Message);
    }
}