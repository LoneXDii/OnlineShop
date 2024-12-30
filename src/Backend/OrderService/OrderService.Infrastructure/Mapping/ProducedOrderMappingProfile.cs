using AutoMapper;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Models;

namespace OrderService.Infrastructure.Mapping;

internal class ProducedOrderMappingProfile : Profile
{
    public ProducedOrderMappingProfile()
    {
        CreateMap<OrderEntity, ProducedOrderDTO>();
    }
}
