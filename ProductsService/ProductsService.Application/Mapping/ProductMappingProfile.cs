using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping;

internal class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.Category, opt => 
                opt.MapFrom(src => 
                    src.CategoryProducts.First(c => c.Category.ParentId == null).Category))
            .ForMember(dest => dest.AttributeValues, opt => 
                opt.MapFrom(src => 
                    src.CategoryProducts.Where(c => c.Category.Children.Count == 0).Select(cp => cp.Category)));
    }
}
