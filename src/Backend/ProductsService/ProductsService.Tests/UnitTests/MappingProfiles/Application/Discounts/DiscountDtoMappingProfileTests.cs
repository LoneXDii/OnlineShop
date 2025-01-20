using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Application.Mapping.Discounts;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;

namespace ProductsService.Tests.UnitTests.MappingProfiles.Application.Discounts;

public class DiscountDtoMappingProfileTests
{
    private readonly IMapper _mapper;

    public DiscountDtoMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DiscountDtoMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenDiscountIsNotNull_ShouldMapToDiscountDTO()
    {
        //Arrange
        var discount = EntityFactory.CreateDiscount();
        
        //Act
        var discountDto = _mapper.Map<DiscountDTO>(discount);
        
        //Assert
        Assert.NotNull(discountDto);
        Assert.Equal(discount.Id, discountDto.Id);
        Assert.Equal(discount.StartDate, discountDto.StartDate);
        Assert.Equal(discount.EndDate, discountDto.EndDate);
        Assert.Equal(discount.Percent, discountDto.Percent);
    }

    [Fact]
    public void Map_WhenDiscountIsNull_ShouldReturnNull()
    {
        //Arrange
        Discount? discount = null;
        
        //Act
        var discountDto = _mapper.Map<DiscountDTO>(discount);
        
        //Assert
        Assert.Null(discountDto);
    }
}