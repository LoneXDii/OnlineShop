using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Application.UseCases.ProductUseCases.Commands.AddProduct;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping.Products;

internal class AddProductRequestMappingProfile : Profile
{
    public AddProductRequestMappingProfile()
    {
        CreateMap<AddProductDTO, AddProductRequest>()
            .ForMember(dest => dest.ImageContentType, opt => opt.MapFrom(src => src.Image != null ? src.Image.ContentType : null));

        CreateMap<AddProductRequest, Product>()
            .ForMember(dest => dest.Categories, opt =>
                opt.MapFrom(src => src.Attributes.Select(attr => new Category { Id = attr })));
    }
}
