﻿using AutoMapper;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddCategory;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping.Categories;

internal class AddCategoryRequestMappingProfile : Profile
{
    public AddCategoryRequestMappingProfile()
    {
        CreateMap<AddCategoryRequest, Category>();
    }
}
