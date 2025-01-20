using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Application.Mapping.Categories;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;

namespace ProductsService.Tests.UnitTests.MappingProfiles.Application.Categories;

public class CategoryAttributeValuesMappingProfileTests
{
    private readonly IMapper _mapper;

    public CategoryAttributeValuesMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CategoryAttributeValuesMappingProfile>();
            cfg.AddProfile<CategoryMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenCategoryIsNotNull_ShouldMapToCategoryAttributesValuesDTO()
    {
        //Arrange
        var childCategory1 = EntityFactory.CreateCategory();
        var childCategory2 = EntityFactory.CreateCategory();
        
        var category = EntityFactory.CreateCategory();

        category.Children.Add(childCategory1);
        category.Children.Add(childCategory2);
        
        //Act
        var dto = _mapper.Map<CategoryAttributesValuesDTO>(category);
        
        //Assert
        Assert.NotNull(dto);
        Assert.NotNull(dto.Attribute);
        Assert.Equal(category.Id, dto.Attribute.Id);
        Assert.Equal(category.Name, dto.Attribute.Name);
        Assert.Equal(2, dto.Values.Count);
        Assert.Contains(dto.Values, v => v.Id == childCategory1.Id);
        Assert.Contains(dto.Values, v => v.Id == childCategory2.Id);
    }

    [Fact]
    public void Map_WhenCategoryIsNull_ShouldReturnNull()
    {
        // Arrange
        Category? category = null;
        
        // Act
        var dto = _mapper.Map<CategoryAttributesValuesDTO>(category);
        
        // Assert
        Assert.Null(dto);
    }
}