using AutoMapper;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddAttribute;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping;

internal class AddAttributeRequestMappingProfile : Profile
{
    public AddAttributeRequestMappingProfile()
    {
        CreateMap<AddAttributeRequest, Category>();
    }
}
