﻿using AutoMapper;
using ProductsService.Application.UseCases.DiscountUseCases.Commands.AddDiscount;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping.Discounts;

internal class AddDiscountRequestMappingProfile : Profile
{
    public AddDiscountRequestMappingProfile()
    {
        CreateMap<AddDiscountRequest, Discount>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
