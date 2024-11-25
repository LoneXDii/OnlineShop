using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping;

internal class ProductAttributeMappingProfile : Profile
{
    public ProductAttributeMappingProfile()
    {
        CreateMap<ProductAttribute, AttributeValueDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Attribute.Name))
            .ForMember(dest => dest.AttributeId, opt => opt.MapFrom(src => src.AttributeId));

        CreateMap<AttributeValueDTO, ProductAttribute>()
            .ForMember(dest => dest.Attribute, opt => opt.Ignore())
            .ForMember(dest => dest.AttributeId, opt => opt.MapFrom(src => src.AttributeId));

        CreateMap<AddProductAttributeDTO, ProductAttribute>();
    }
}
