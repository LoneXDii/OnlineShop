using AutoMapper;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Models;

namespace OrderService.Infrastructure.Mapping;

internal class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<OrderEntity, Order>()
            .ReverseMap();
    }
}
