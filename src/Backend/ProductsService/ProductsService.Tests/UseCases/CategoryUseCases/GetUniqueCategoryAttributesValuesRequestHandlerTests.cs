using System.Linq.Expressions;
using AutoFixture;
using AutoMapper;
using Moq;
using ProductsService.Application.DTO;
using ProductsService.Application.Specifications.Categories;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetUniqueCategoryAttributesValues;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UseCases.CategoryUseCases;

public class GetUniqueCategoryAttributesValuesRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Fixture _fixture = new();
    private readonly GetUniqueCategoryAttributesValuesRequestHandler _handler;

    public GetUniqueCategoryAttributesValuesRequestHandlerTests()
    {
        _handler = new GetUniqueCategoryAttributesValuesRequestHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldReturnAttributesValuesForCategory()
    {
        //Arrange
        var request = _fixture.Create<GetUniqueCategoryAttributesValuesRequest>();
        var categories = new List<Category>();
        var categoryAttributesValues = _fixture.CreateMany<CategoryAttributesValuesDTO>().ToList();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.ListAsync(It.IsAny<ParentCategorySpecification>(), It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Category, object>>>()))
            .ReturnsAsync(categories);

        _mapperMock.Setup(mapper => mapper.Map<List<CategoryAttributesValuesDTO>>(categories))
            .Returns(categoryAttributesValues);

        //Act
        var result = await _handler.Handle(request, default);

        //Assert
        Assert.Equal(categoryAttributesValues, result);
        
        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.CategoryQueryRepository.ListAsync(It.IsAny<ParentCategorySpecification>(), It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Category, object>>>()), Times.Once);
        
        _mapperMock.Verify(mapper => 
            mapper.Map<List<CategoryAttributesValuesDTO>>(categories), Times.Once);
    }
    
}