using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping.Categories;

internal class CategoryAttributeValuesMappingProfile : Profile
{
    public CategoryAttributeValuesMappingProfile()
    {
        CreateMap<Category, CategoryAttributesValuesDTO>()
            .ForMember(dest => dest.Attribute, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Values, opt => opt.MapFrom(src => src.Children));
    }
}
