using AutoMapper;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Protos;

namespace OrderService.Infrastructure.Mapping;

internal class ProductResponseMappingProfile : Profile
{
    public ProductResponseMappingProfile()
    {
        CreateMap<ProductResponse, ProductEntity>();
    }
}
