using AutoFixture;
using AutoMapper;
using ProductsService.Application.Mapping.Discounts;
using ProductsService.Application.UseCases.DiscountUseCases.Commands.AddDiscount;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UnitTests.MappingProfiles.Application.Discounts;

public class AddDiscountRequestMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public AddDiscountRequestMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<AddDiscountRequestMappingProfile>(); });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenAddDiscountRequestIsNotNull_ShouldMapToDiscount()
    {
        //Arrange
        var addDiscountRequest = _fixture.Create<AddDiscountRequest>();

        //Act
        var discount = _mapper.Map<Discount>(addDiscountRequest);

        //Assert
        Assert.NotNull(discount);
        Assert.Equal(addDiscountRequest.ProductId, discount.ProductId);
        Assert.Equal(addDiscountRequest.StartDate, discount.StartDate);
        Assert.Equal(addDiscountRequest.EndDate, discount.EndDate);
        Assert.Equal(addDiscountRequest.Percent, discount.Percent);
    }

    [Fact]
    public void Map_WhenAddDiscountRequestIsNull_ShouldReturnNull()
    {
        //Arrange
        AddDiscountRequest? addDiscountRequest = null;

        //Act
        var discount = _mapper.Map<Discount>(addDiscountRequest);

        //Assert
        Assert.Null(discount);
    }
}
