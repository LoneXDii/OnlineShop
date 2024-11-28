using AutoMapper;
using OrderService.Application.DTO;
using OrderService.Domain.Entities;

namespace OrderService.Application.Mapping;

internal class CartMappingProfile : Profile
{
    public CartMappingProfile()
    {
        CreateMap<Dictionary<int, ProductEntity>, CartDTO>()
            .ForMember(dest => dest.Products,
                opt => opt.MapFrom(src => src.Values.ToList()))
            .ForMember(dest => dest.Count,
                opt => opt.MapFrom(src => src.Sum(item => item.Value.Quantity)))
            .ForMember(dest => dest.TotalCost,
                opt => opt.MapFrom(src => src.Sum(item => item.Value.Price * item.Value.Quantity)));
    }
}
