using AutoMapper;
using OrderService.Application.DTO;
using OrderService.Application.UseCases.OrderUseCases.GetAllOrdersUseCase;

namespace OrderService.Application.Mapping;

internal class GetAllOrdersRequestMappingProfile : Profile
{
    public GetAllOrdersRequestMappingProfile()
    {
        CreateMap<PaginationDTO, GetAllOrdersRequest>();
    }
}
