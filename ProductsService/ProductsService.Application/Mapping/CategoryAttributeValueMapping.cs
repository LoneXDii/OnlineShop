using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping;

internal class CategoryAttributeValueMapping : Profile
{
    public CategoryAttributeValueMapping()
    {
        CreateMap<Category, CategoryAttributesValuesDTO>()
            .ForMember(dest => dest.Attribute, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Values, opt => opt.MapFrom(src => src.Children));
    }
}
