using AutoMapper;
using OrderService.Application.DTO;
using OrderService.Domain.Abstractions.Cart;
namespace OrderService.Application.Mapping;

internal class CartMappingProfile : Profile
{
    public CartMappingProfile()
    {
        CreateMap<ICart, CartDTO>()
            .ForMember(dest => dest.Products,
                opt => opt.MapFrom(src => src.Items.Values.ToList()));
    }
}
