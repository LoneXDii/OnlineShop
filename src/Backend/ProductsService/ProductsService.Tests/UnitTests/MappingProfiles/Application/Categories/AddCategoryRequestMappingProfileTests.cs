using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using ProductsService.Application.DTO;
using ProductsService.Application.Mapping.Categories;
using ProductsService.Application.Mapping.Common;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddCategory;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UnitTests.MappingProfiles.Application.Categories;

public class AddCategoryRequestMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IFormFile> _formFileMock = new();
    private readonly Mock<Stream> _streamMock = new();
    private readonly Fixture _fixture = new();

    public AddCategoryRequestMappingProfileTests()
    {
        _formFileMock.Setup(file => file.OpenReadStream()).Returns(_streamMock.Object);
        _formFileMock.Setup(file => file.ContentType).Returns("content-type");
        _streamMock.Setup(stream => stream.Length).Returns(_fixture.Create<int>());
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AddCategoryRequestMappingProfile>();
            cfg.AddProfile<FormFileToStreamMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }
    
    [Fact]
    public void Map_WhenAddCategoryDTOIsNull_ShouldReturnNull()
    {
        //Arrange
        AddCategoryDTO? requestDto = null;
        
        //Act
        var request = _mapper.Map<AddCategoryRequest>(requestDto);
        
        //Assert
        Assert.Null(request);
    }
    
    [Fact]
    public void Map_WhenImageIsNotNull_ShouldCreateStreamAndSetContentType()
    {
        //Arrange
        var requestDto = _fixture.Build<AddCategoryDTO>()
            .With(dto => dto.Image, _formFileMock.Object)
            .Create();
        
        //Act
        var request = _mapper.Map<AddCategoryRequest>(requestDto);
        
        //Assert
        Assert.NotNull(request);
        Assert.Equal(_formFileMock.Object.ContentType, request.ImageContentType);
        Assert.Equal(_streamMock.Object, request.Image);
    }
    
    [Fact]
    public void Map_WhenImageIsNull_ShouldSetImageContentTypeToNull()
    {
        //Arrange
        var requestDto = _fixture.Build<AddCategoryDTO>()
            .With(dto => dto.Image, (IFormFile?)null)
            .Create();
        
        //Act
        var request = _mapper.Map<AddCategoryRequest>(requestDto);
        
        //Assert
        Assert.NotNull(request);
        Assert.Null(request.ImageContentType);
    }

    [Fact]
    public void Map_WhenAddCategoryRequestIsNotNull_ShouldMapToCategory()
    {
        //Arrange
        var request = new AddCategoryRequest("Test Category", _streamMock.Object, "image/jpeg");
        
        //Act
        var category = _mapper.Map<Category>(request);
        
        //Assert
        Assert.NotNull(category);
        Assert.Equal(request.Name, category.Name);
    }
    
    [Fact]
    public void Map_WhenAddCategoryRequestIsNull_ShouldReturnNull()
    {
        //Arrange
        AddCategoryRequest? request = null;
        
        //Act
        var category = _mapper.Map<Category>(request);
        
        //Assert
        Assert.Null(category);
    }
}