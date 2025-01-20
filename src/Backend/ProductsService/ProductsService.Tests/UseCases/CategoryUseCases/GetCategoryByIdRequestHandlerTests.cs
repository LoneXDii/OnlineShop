using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.DTO;
using ProductsService.Application.Exceptions;
using ProductsService.Application.UseCases.CategoryUseCases.Queries.GetCategoryById;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;

namespace ProductsService.Tests.UseCases.CategoryUseCases;

public class GetCategoryByIdRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<GetCategoryByIdRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly GetCategoryByIdRequestHandler _handler;

    public GetCategoryByIdRequestHandlerTests()
    {
        _handler = new GetCategoryByIdRequestHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoryExists_ShouldReturnMappedCategory()
    {
        //Arrange
        var category = EntityFactory.CreateCategory();
        var responseCategoryDto = _fixture.Create<ResponseCategoryDTO>();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _mapperMock.Setup(mapper => mapper.Map<ResponseCategoryDTO>(category))
            .Returns(responseCategoryDto);

        var request = new GetCategoryByIdRequest(category.Id);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.Equal(responseCategoryDto, result);
        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.CategoryQueryRepository.GetByIdAsync(category.Id, It.IsAny<CancellationToken>()), Times.Once);
        
        _mapperMock.Verify(mapper => 
            mapper.Map<ResponseCategoryDTO>(category), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _fixture.Create<GetCategoryByIdRequest>();
        
        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(request.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal("No such category", exception.Message);
        
        //Assert
        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.CategoryQueryRepository.GetByIdAsync(request.CategoryId, It.IsAny<CancellationToken>()), Times.Once);
        
        _mapperMock.Verify(mapper => 
            mapper.Map<ResponseCategoryDTO>(It.IsAny<Category>()), Times.Never);
    }
}