using AutoFixture;
using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Application.Mapping.Categories;
using ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;

namespace ProductsService.Tests.UnitTests.MappingProfiles.Application.Categories;

public class UpdateProductAttributeRequestMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public UpdateProductAttributeRequestMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UpdateProductAttributeRequestMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenRequestAttributeValueDTOIsNotNull_ShouldMapToUpdateProductAttributeRequest()
    {
        //Arrange
        var requestDto = _fixture.Create<RequestAttributeValueDTO>();
        
        //Act
        var request = _mapper.Map<UpdateProductAttributeRequest>(requestDto);
        
        //Assert
        Assert.NotNull(request);
        Assert.Equal(requestDto.ProductId, request.ProductId);
        Assert.Equal(requestDto.AttributeId, request.OldAttributeId);
        Assert.Equal(0, request.NewAttributeId); 
    }

    [Fact]
    public void Map_WhenRequestAttributeValueDTOIsNull_ShouldReturnNull()
    {
        //Arrange
        RequestAttributeValueDTO? requestDto = null;
        
        //Act
        var request = _mapper.Map<UpdateProductAttributeRequest>(requestDto);
        
        //Assert
        Assert.Null(request);
    }
}