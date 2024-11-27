using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping;

internal class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Categories.First(c => c.ParentId == null)))
            .ForMember(dest => dest.AttributeValues, opt => opt.MapFrom(src => src.Categories.Where(c => c.Children == null)));
    }
}
