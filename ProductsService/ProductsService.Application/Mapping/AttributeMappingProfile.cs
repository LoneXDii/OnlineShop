using AutoMapper;
using ProductsService.Application.DTO;
using Attribute = ProductsService.Domain.Entities.Attribute;

namespace ProductsService.Application.Mapping;

internal class AttributeMappingProfile : Profile
{
    public AttributeMappingProfile()
    {
        CreateMap<Attribute, AttributeDTO>()
            .ReverseMap();
    }
}
