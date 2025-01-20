using System.Linq.Expressions;
using AutoMapper;
using Moq;
using ProductsService.Application.DTO;
using ProductsService.Application.Specifications.Categories;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetAllCategories;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UnitTests.UseCases.CategoryUseCases;

public class GetAllCategoriesRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly GetAllCategoriesRequestHandler _handler;

    public GetAllCategoriesRequestHandlerTests()
    {
        _handler = new GetAllCategoriesRequestHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldReturnAllCategories()
    {
        //Arrange
        var request = new GetAllCategoriesReguest();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.ListAsync(It.IsAny<TopLevelCategorySpecification>(), It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Category, object>>>()))
            .ReturnsAsync(new List<Category>());

        _mapperMock.Setup(mapper => mapper.Map<List<ResponseCategoryDTO>>(It.IsAny<List<Category>>()))
            .Returns(new List<ResponseCategoryDTO>());

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
    }
}