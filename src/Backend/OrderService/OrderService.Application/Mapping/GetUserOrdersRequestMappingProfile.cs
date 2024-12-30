using AutoMapper;
using OrderService.Application.DTO;
using OrderService.Application.UseCases.OrderUseCases.GetUserOrdersUseCase;

namespace OrderService.Application.Mapping;

internal class GetUserOrdersRequestMappingProfile : Profile
{
    public GetUserOrdersRequestMappingProfile()
    {
        CreateMap<PaginationDTO, GetUserOrdersRequest>();
    }
}
