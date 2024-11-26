using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping;

internal class CategoryAttributesValuesMappingProfile : Profile
{
    public CategoryAttributesValuesMappingProfile()
    {
        CreateMap<ProductAttribute, CategoryAttributesValuesDTO>()
            .ForMember(dest => dest.AttributeName, opt => opt.MapFrom(src => src.Attribute.Name))
            .ForMember(dest => dest.Values, opt => opt.Ignore());

        CreateMap<List<ProductAttribute>, List<CategoryAttributesValuesDTO>>()
            .ConvertUsing(src =>
                src.GroupBy(pa => pa.Attribute.Name)
                   .Select(group => new CategoryAttributesValuesDTO
                   {
                       AttributeName = group.Key,
                       Values = group.Select(pa => pa.Value).ToList()
                   })
                   .ToList());
    }
}
