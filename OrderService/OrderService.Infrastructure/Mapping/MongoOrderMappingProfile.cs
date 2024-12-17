using AutoMapper;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Models;
namespace OrderService.Infrastructure.Mapping;

internal class MongoOrderMappingProfile : Profile
{
    public MongoOrderMappingProfile()
    {
        CreateMap<OrderEntity, MongoOrder>()
            .ReverseMap();
    }
}
