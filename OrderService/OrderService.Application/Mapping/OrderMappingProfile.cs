using AutoMapper;
using OrderService.Application.DTO;
using OrderService.Domain.Abstractions.Cart;
using OrderService.Application.Extensions;
using OrderService.Domain.Entities;

namespace OrderService.Application.Mapping;

internal class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<OrderEntity, OrderDTO>()
            .ForMember(dest => dest.OrderStatusDescription,
                opt => opt.MapFrom(src => src.OrderStatus.GetDescription()))
            .ForMember(dest => dest.PaymentStatusDescription,
                opt => opt.MapFrom(src => src.PaymentStatus.GetDescription()));
    }
}
