using AutoMapper;
using ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProduct;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping.Products;

internal class UpdateProductRequestMappingProfile : Profile
{
    public UpdateProductRequestMappingProfile()
    {
        CreateMap<UpdateProductRequest, Product>()
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .ForMember(dest => dest.Price, opt => opt.Ignore())
            .ForMember(dest => dest.Quantity, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Name = src.Name ?? dest.Name;
                dest.Price = src.Price ?? dest.Price;
                dest.Quantity = src.Quantity ?? dest.Quantity;
            });
    }
}
