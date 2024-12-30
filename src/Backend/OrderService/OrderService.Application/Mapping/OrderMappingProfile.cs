using AutoMapper;
using OrderService.Application.DTO;
using OrderService.Domain.Entities;

namespace OrderService.Application.Mapping;

internal class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<OrderEntity, OrderDTO>();
    }
}
