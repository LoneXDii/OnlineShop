using AutoFixture;
using AutoMapper;
using OrderService.Application.DTO;
using OrderService.Application.Mapping;
using OrderService.Application.UseCases.OrderUseCases.GetUserOrdersUseCase;

namespace OrderService.Tests.UnitTests.MappingProfiles.Application;

public class GetUserOrdersRequestMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public GetUserOrdersRequestMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GetUserOrdersRequestMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenPaginationDTOIsNotNull_ShouldReturnCorrectGetUserOrdersRequest()
    {
        //Arrange
        var paginationDTO = _fixture.Create<PaginationDTO>();

        //Act
        var getUserOrdersRequest = _mapper.Map<GetUserOrdersRequest>(paginationDTO);

        //Assert
        Assert.NotNull(getUserOrdersRequest);
        Assert.Equal(paginationDTO.PageNo, getUserOrdersRequest.PageNo);
        Assert.Equal(paginationDTO.PageSize, getUserOrdersRequest.PageSize);
        Assert.Equal("", getUserOrdersRequest.UserId);
    }

    [Fact]
    public void Map_WhenPaginationDTOIsNull_ShouldReturnNull()
    {
        //Act
        var getUserOrdersRequest = _mapper.Map<GetUserOrdersRequest>(null);

        //Assert
        Assert.Null(getUserOrdersRequest);
    }
}
