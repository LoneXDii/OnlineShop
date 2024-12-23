using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddCategory;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping.Categories;

internal class AddCategoryRequestMappingProfile : Profile
{
    public AddCategoryRequestMappingProfile()
    {
        CreateMap<AddCategoryDTO, AddCategoryRequest>()
            .ForMember(dest => dest.ImageContentType, opt => opt.MapFrom(src => src.Image != null ? src.Image.ContentType : null));

        CreateMap<AddCategoryRequest, Category>();
    }
}
