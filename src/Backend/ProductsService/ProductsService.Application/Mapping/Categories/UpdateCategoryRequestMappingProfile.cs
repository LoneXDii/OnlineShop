using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.UpdateCategory;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping.Categories;

internal class UpdateCategoryRequestMappingProfile : Profile
{
    public UpdateCategoryRequestMappingProfile()
    {
        CreateMap<UpdateCategoryRequest, Category>();

        CreateMap<UpdateCategoryDTO, UpdateCategoryRequest>()
            .ForMember(dest => dest.ImageContentType, opt => opt.MapFrom(src => src.Image != null ? src.Image.ContentType : null));
    }
}
