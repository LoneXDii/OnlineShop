using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping.Discounts;

internal class DiscountDtoMappingProfile : Profile
{
    public DiscountDtoMappingProfile()
    {
        CreateMap<Discount, DiscountDTO>();
    }
}
