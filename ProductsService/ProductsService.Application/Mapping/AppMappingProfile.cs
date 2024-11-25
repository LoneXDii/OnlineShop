using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Application.Models;
using ProductsService.Domain.Entities;
using Attribute = ProductsService.Domain.Entities.Attribute;

namespace ProductsService.Application.Mapping;

internal class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Category, CategoryDTO>();

        CreateMap<ProductAttribute, AttributeValueDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Attribute.Name))
            .ForMember(dest => dest.AttributeId, opt => opt.MapFrom(src => src.AttributeId));

        CreateMap<AttributeValueDTO, ProductAttribute>()
            .ForMember(dest => dest.Attribute, opt => opt.Ignore())
            .ForMember(dest => dest.AttributeId, opt => opt.MapFrom(src => src.AttributeId));

        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.AttributeValues, opt => opt.MapFrom(src => src.ProductAttributes));

        CreateMap<PostProductDTO, Product>()
            .ForMember(dest => dest.ProductAttributes, opt => opt.MapFrom(src => src.AttributeValues));

        CreateMap<AddProductAttributeDTO, ProductAttribute>();
    }
}
