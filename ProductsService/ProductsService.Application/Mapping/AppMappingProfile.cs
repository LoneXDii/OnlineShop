using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Domain.Entities;
using Attribute = ProductsService.Domain.Entities.Attribute;

namespace ProductsService.Application.Mapping;

internal class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Category, CategoryDTO>();

        CreateMap<ProductAttribute, AttributeValueDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Attribute.Name));

        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.AttributeValues, opt => opt.MapFrom(src => src.ProductAttributes));
    }
}
