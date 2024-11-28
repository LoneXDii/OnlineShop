using AutoMapper;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.UpdateCategory;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping.Categories;

internal class UpdateCategoryRequestMappingProfile : Profile
{
    public UpdateCategoryRequestMappingProfile()
    {
        CreateMap<UpdateCategoryRequest, Category>();
    }
}
