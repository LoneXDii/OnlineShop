using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping;

internal class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        //CreateMap<Product, ProductDTO>()
        //    .ForMember(dest => dest.AttributeValues, opt => opt.MapFrom(src => src.ProductAttributes));

        //CreateMap<PostProductDTO, Product>()
        //    .ForMember(dest => dest.ProductAttributes, opt => opt.MapFrom(src => src.AttributeValues));

        //CreateMap<UpdateProductDTO, Product>();
    }
}
