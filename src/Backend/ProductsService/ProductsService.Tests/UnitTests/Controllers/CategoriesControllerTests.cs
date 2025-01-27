using AutoFixture;
using AutoMapper;
using MediatR;
using Moq;
using ProductsService.API.Controllers;
using ProductsService.Application.DTO;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddAttribute;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddCategory;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.DeleteCategory;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.UpdateCategory;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetAllCategories;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetCategoryAttributes;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetCategoryById;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetUniqueCategoryAttributesValues;

namespace ProductsService.Tests.UnitTests.Controllers;

public class CategoriesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CategoriesController _controller;
    private readonly Fixture _fixture = new();
    
    public CategoriesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _controller = new CategoriesController(_mediatorMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetCategories_WhenCalled_ShouldCallGetAllCategoriesRequest()
    {
        //Act
        await _controller.GetCategories(CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllCategoriesReguest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetCategory_WhenCalled_ShouldCallGetCategoryByIdRequest()
    {
        //Arrange
        var request = _fixture.Create<GetCategoryByIdRequest>();

        //Act
        await _controller.GetCategory(request, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task AddCategory_WhenCalled_ShouldCallAddCategoryRequest()
    {
        //Arrange
        var category = new AddCategoryDTO();
        var request = new AddCategoryRequest("", null, null);

        _mapperMock.Setup(m => m.Map<AddCategoryRequest>(category)).Returns(request);

        //Act
        await _controller.AddCategory(category, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task UpdateCategory_WhenCalled_ShouldCallUpdateCategoryRequest()
    {
        //Arrange
        var categoryDTO = new UpdateCategoryDTO();
        var request = new UpdateCategoryRequest();
        var categoryId = 1;

        _mapperMock.Setup(m => m.Map<UpdateCategoryRequest>(categoryDTO)).Returns(request);

        //Act
        await _controller.UpdateCategory(categoryId, categoryDTO, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task DeleteCategory_WhenCalled_ShouldCallDeleteCategoryRequest()
    {
        //Arrange
        var request = _fixture.Create<DeleteCategoryRequest>();

        //Act
        await _controller.DeleteCategory(request, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetCategoryAttributes_WhenCalled_ShouldCallGetCategoryAttributesRequest()
    {
        //Arrange
        var request = _fixture.Create<GetCategoryAttributesRequest>();

        //Act
        await _controller.GetCategoryAttributes(request, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task AddCategoryAttribute_WhenCalled_ShouldCallAddAttributeRequest()
    {
        //Arrange
        var categoryId = 1;
        var categoryName = new CategoryNameDTO { Name = "Test Attribute" };

        //Act
        await _controller.AddCategoryAttribute(categoryId, categoryName, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<AddAttributeRequest>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetCategoryAttributesValues_WhenCalled_ShouldCallGetUniqueCategoryAttributesValuesRequest()
    {
        //Arrange
        var request = _fixture.Create<GetUniqueCategoryAttributesValuesRequest>();

        //Act
        await _controller.GetCategoryAttributesValues(request, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }
}