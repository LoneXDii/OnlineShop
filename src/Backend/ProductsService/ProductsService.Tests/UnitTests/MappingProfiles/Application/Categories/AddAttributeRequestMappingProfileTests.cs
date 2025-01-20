using AutoFixture;
using AutoMapper;
using ProductsService.Application.Mapping.Categories;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddAttribute;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UnitTests.MappingProfiles.Application.Categories;

public class AddAttributeRequestMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public AddAttributeRequestMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AddAttributeRequestMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenAddAttributeRequestIsNotNull_ShouldMapToCategory()
    {
        //Arrange
        var request = _fixture.Create<AddAttributeRequest>();
        
        //Act
        var category = _mapper.Map<Category>(request);
        
        //Assert
        Assert.NotNull(category);
        Assert.Equal(request.ParentId, category.ParentId);
        Assert.Equal(request.Name, category.Name);
    }
    
    [Fact]
    public void Map_WhenAddAttributeRequestIsNull_ShouldReturnNull()
    {
        //Arrange
        AddAttributeRequest? request = null;
        
        //Act
        var category = _mapper.Map<Category>(request);
        
        //Assert
        Assert.Null(category);
    }
}