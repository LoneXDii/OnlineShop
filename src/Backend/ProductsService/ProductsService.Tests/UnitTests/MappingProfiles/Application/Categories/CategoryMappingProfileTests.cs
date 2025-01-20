using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Application.Mapping.Categories;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;

namespace ProductsService.Tests.UnitTests.MappingProfiles.Application.Categories;

public class CategoryMappingProfileTests
{
    private readonly IMapper _mapper;

    public CategoryMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CategoryMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenCategoryIsNotNull_ShouldMapToResponseCategoryDTO()
    {
        // Arrange
        var category = EntityFactory.CreateCategory();
        
        // Act
        var dto = _mapper.Map<ResponseCategoryDTO>(category);
        
        // Assert
        Assert.NotNull(dto);
        Assert.Equal(category.Id, dto.Id);
        Assert.Equal(category.Name, dto.Name);
        Assert.Equal(category.ImageUrl, dto.ImageUrl);
    }

    [Fact]
    public void Map_WhenCategoryIsNull_ShouldReturnNull()
    {
        // Arrange
        Category? category = null;
        
        // Act
        var dto = _mapper.Map<ResponseCategoryDTO>(category);
        
        // Assert
        Assert.Null(dto);
    }
}