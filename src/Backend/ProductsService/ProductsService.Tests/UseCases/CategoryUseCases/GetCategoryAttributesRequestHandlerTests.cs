using AutoFixture;
using AutoMapper;
using Moq;
using ProductsService.Application.DTO;
using ProductsService.Application.Specifications.Categories;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetCategoryAttributes;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UseCases.CategoryUseCases;

public class GetCategoryAttributesRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Fixture _fixture = new();
    private readonly GetCategoryAttributesRequestHandler _handler;

    public GetCategoryAttributesRequestHandlerTests()
    {
        _handler = new GetCategoryAttributesRequestHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldReturnListOfChildCategories()
    {
        //Arrange
        var request = _fixture.Create<GetCategoryAttributesRequest>();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.ListAsync(It.IsAny<ParentCategorySpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Category>());
        
        _mapperMock.Setup(mapper => mapper.Map<List<ResponseCategoryDTO>>(It.IsAny<List<Category>>()))
            .Returns(new List<ResponseCategoryDTO>());
        
        //Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        //Assert
        Assert.NotNull(result);
    }
}