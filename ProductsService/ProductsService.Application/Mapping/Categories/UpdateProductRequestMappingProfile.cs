using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProduct;

namespace ProductsService.Application.Mapping.Categories;

internal class UpdateProductRequestMappingProfile : Profile
{
    public UpdateProductRequestMappingProfile()
    {
        CreateMap<RequestProductDTO, UpdateProductRequest>();
    }
}
