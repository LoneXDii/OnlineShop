using AutoMapper;
using OrderService.Application.DTO;
using OrderService.Domain.Abstractions.Cart;
using OrderService.Domain.Common.Models;
namespace OrderService.Application.Mapping;

internal class CartMappingProfile : Profile
{
    public CartMappingProfile()
    {
        CreateMap<Cart, CartDTO>()
            .ForMember(dest => dest.Products,
                opt => opt.MapFrom(src => src.Items.Values.ToList()));
    }
}
