using AutoMapper;
using ProductsService.API.Protos;
using ProductsService.Domain.Entities;

namespace ProductsService.API.Mapping;

public class ProductResponseMappingProfile : Profile
{
    public ProductResponseMappingProfile()
    {
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl ?? ""))
            .ForMember(dest => dest.PriceId, opt => opt.MapFrom(src => src.PriceId ?? ""));
    }
}
