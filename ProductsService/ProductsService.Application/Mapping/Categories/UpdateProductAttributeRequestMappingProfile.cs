using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;

namespace ProductsService.Application.Mapping.Categories;

internal class UpdateProductAttributeRequestMappingProfile : Profile
{
    public UpdateProductAttributeRequestMappingProfile()
    {
        CreateMap<RequestAttributeValueDTO, UpdateProductAttributeRequest>()
            .ForMember(dest => dest.OldAttributeId, opt => opt.MapFrom(src => src.AttributeId));
    }
}
