using AutoMapper;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Protos;

namespace OrderService.Infrastructure.Mapping;

internal class ProductRequestMappingProfile : Profile
{
    public ProductRequestMappingProfile()
    {
        CreateMap<ProductEntity, ProductRequest>();
    }
}
