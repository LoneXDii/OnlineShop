using AutoFixture;
using AutoMapper;
using OrderService.Application.DTO;
using OrderService.Application.Mapping;
using OrderService.Application.UseCases.OrderUseCases.GetAllOrdersUseCase;

namespace OrderService.Tests.UnitTests.MappingProfiles.Application;

public class GetAllOrdersRequestMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public GetAllOrdersRequestMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GetAllOrdersRequestMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenPaginationDTOIsNotNull_ShouldReturnCorrectGetAllOrdersRequest()
    {
        //Arrange
        var paginationDTO = _fixture.Create<PaginationDTO>();

        //Act
        var getAllOrdersRequest = _mapper.Map<GetAllOrdersRequest>(paginationDTO);

        //Assert
        Assert.NotNull(getAllOrdersRequest);
        Assert.Equal(paginationDTO.PageNo, getAllOrdersRequest.PageNo);
        Assert.Equal(paginationDTO.PageSize, getAllOrdersRequest.PageSize);
    }

    [Fact]
    public void Map_WhenPaginationDTOIsNull_ShouldReturnNull()
    {
        //Act
        var getAllOrdersRequest = _mapper.Map<GetAllOrdersRequest>(null);

        //Assert
        Assert.Null(getAllOrdersRequest);
    }
}
