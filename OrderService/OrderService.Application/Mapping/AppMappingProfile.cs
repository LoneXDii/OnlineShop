using AutoMapper;
using OrderService.Application.DTO;
using OrderService.Application.Extensions;
using OrderService.Domain.Common.Statuses;
using OrderService.Domain.Entities;
using System;

namespace OrderService.Application.Mapping;

internal class AppMappingProfile : Profile
{
	public AppMappingProfile()
	{
		CreateMap<Order, GetOrderDTO>()
			.ForMember(dest => dest.OrderStatusDescription,
				opt => opt.MapFrom(src => src.OrderStatus.GetDescription()))
			.ForMember(dest => dest.PaymentStatusDescription,
				opt => opt.MapFrom(src => src.PaymentStatus.GetDescription()));

		CreateMap<PostOrderDTO, Order>()
			.ForMember(dest => dest.OrderStatus,
				opt => opt.MapFrom(src => (OrderStatuses)src.OrderStatus))
			.ForMember(dest => dest.PaymentStatus,
				opt => opt.MapFrom(src => (PaymentStatuses)src.PaymentStatus));
	}
}
