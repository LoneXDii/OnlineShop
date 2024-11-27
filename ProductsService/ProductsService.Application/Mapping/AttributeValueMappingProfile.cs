using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping;

internal class AttributeValueMappingProfile : Profile
{
    public AttributeValueMappingProfile()
    {
        CreateMap<Category, AttributeValueDTO>()
            .ForMember(dest => dest.AttributeId, opt => opt.MapFrom(src => src.ParentId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Parent.Name))
            .ForMember(dest => dest.ValueId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name));
    }
}
