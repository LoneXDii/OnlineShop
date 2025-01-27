using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using ProductsService.Application.DTO;
using ProductsService.Application.Mapping.Categories;
using ProductsService.Application.Mapping.Common;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.UpdateCategory;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UnitTests.MappingProfiles.Application.Categories;

public class UpdateCategoryRequestMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Mock<Stream> _streamMock = new();
    private readonly Fixture _fixture = new();

    public UpdateCategoryRequestMappingProfileTests()
    {
        _streamMock.Setup(stream => stream.Length).Returns(_fixture.Create<int>());
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UpdateCategoryRequestMappingProfile>();
            cfg.AddProfile<FormFileToStreamMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenUpdateCategoryDTOIsNotNull_ShouldMapToUpdateCategoryRequest()
    {
        //Arrange
        var updateCategoryDto = _fixture.Build<UpdateCategoryDTO>()
            .With(dto => dto.Image, new FormFile(_streamMock.Object, 0, _streamMock.Object.Length, "file", "test.jpg"))
            .Create();
        
        //Act
        var request = _mapper.Map<UpdateCategoryRequest>(updateCategoryDto);
        
        //Assert
        Assert.NotNull(request);
        Assert.Equal(updateCategoryDto.Name, request.Name);
        Assert.NotNull(request.Image);
    }
    
    [Fact]
    public void Map_WhenFormFileIsNull_ShouldMapToUpdateCategoryRequestWithoutImage()
    {
        //Arrange
        var updateCategoryDto = _fixture.Build<UpdateCategoryDTO>()
            .With(dto => dto.Image, (IFormFile?)null)
            .Create();
        
        //Act
        var request = _mapper.Map<UpdateCategoryRequest>(updateCategoryDto);
        
        //Assert
        Assert.NotNull(request);
        Assert.Null(request.Image);
    }
    
    [Fact]
    public void Map_WhenUpdateCategoryDTOIsNull_ShouldReturnNull()
    {
        //Arrange
        UpdateCategoryDTO? updateCategoryDto = null;
        
        //Act
        var request = _mapper.Map<UpdateCategoryRequest>(updateCategoryDto);
        
        //Assert
        Assert.Null(request);
    }

    [Fact]
    public void Map_WhenUpdateCategoryRequestIsNotNull_ShouldMapToCategory()
    {
        //Arrange
        var request = _fixture.Build<UpdateCategoryRequest>()
            .With(request => request.Image, (Stream?)null)
            .Create();
        
        //Act
        var category = _mapper.Map<Category>(request);
        
        //Assert
        Assert.NotNull(category);
        Assert.Equal(request.Name, category.Name);
    }

    [Fact]
    public void Map_WhenUpdateCategoryRequestIsNull_ShouldReturnNull()
    {
        //Arrange
        UpdateCategoryRequest? request = null;
        
        //Act
        var category = _mapper.Map<Category>(request);
        
        //Assert
        Assert.Null(category);
    }
}