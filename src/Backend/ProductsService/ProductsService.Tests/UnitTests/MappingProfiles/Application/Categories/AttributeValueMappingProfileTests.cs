using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Application.Mapping.Categories;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;

namespace ProductsService.Tests.UnitTests.MappingProfiles.Application.Categories;

public class AttributeValueMappingProfileTests
{
    private readonly IMapper _mapper;

    public AttributeValueMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AttributeValueMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenCategoryIsNotNull_ShouldMapToResponseAttributeValueDTO()
    {
        //Arrange
        var parentCategory = EntityFactory.CreateCategory();
        var category = EntityFactory.CreateCategory();
        category.Parent = parentCategory;
        
        //Act
        var dto = _mapper.Map<ResponseAttributeValueDTO>(category);
        
        //Assert
        Assert.NotNull(dto);
        Assert.Equal(category.ParentId, dto.AttributeId);
        Assert.Equal(parentCategory.Name, dto.Name);
        Assert.Equal(category.Id, dto.ValueId);
        Assert.Equal(category.Name, dto.Value);
    }
    
    [Fact]
    public void Map_WhenCategoryIsNull_ShouldReturnNull()
    {
        //Arrange
        Category? category = null;
        
        //Act
        var dto = _mapper.Map<ResponseAttributeValueDTO>(category);
        
        //Assert
        Assert.Null(dto);
    }
}