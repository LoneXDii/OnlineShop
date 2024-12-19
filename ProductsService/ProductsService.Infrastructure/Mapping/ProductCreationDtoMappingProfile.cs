using AutoMapper;
using ProductsService.Domain.Entities;
using ProductsService.Infrastructure.Models;

namespace ProductsService.Infrastructure.Mapping;

internal class ProductCreationDtoMappingProfile : Profile
{
    public ProductCreationDtoMappingProfile()
    {
        CreateMap<Product, ProductCreationDTO>();
    }
}
