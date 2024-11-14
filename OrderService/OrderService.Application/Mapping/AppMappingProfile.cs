using AutoMapper;
using OrderService.Application.DTO;
using OrderService.Application.Extensions;
using OrderService.Domain.Abstractions.Cart;
using OrderService.Domain.Common.Models;
using OrderService.Domain.Entities;

namespace OrderService.Application.Mapping;

internal class AppMappingProfile : Profile
{
	public AppMappingProfile()
	{
		CreateMap<OrderEntity, GetOrderDTO>()
			.ForMember(dest => dest.OrderStatusDescription,
				opt => opt.MapFrom(src => src.OrderStatus.GetDescription()))
			.ForMember(dest => dest.PaymentStatusDescription,
				opt => opt.MapFrom(src => src.PaymentStatus.GetDescription()));

		CreateMap<Cart, CartDTO>()
			.ForMember(dest => dest.Products,
				opt => opt.MapFrom(src => src.Items.Values.ToList()));

		CreateMap<PaginatedListModel<OrderEntity>, PaginatedListModel<GetOrderDTO>>();
	}
}
