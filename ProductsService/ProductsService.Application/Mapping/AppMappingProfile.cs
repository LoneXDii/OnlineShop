using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Domain.Entities;
using Attribute = ProductsService.Domain.Entities.Attribute;

namespace ProductsService.Application.Mapping;

internal class AppMappingProfile : Profile
{
	public AppMappingProfile()
	{
	}
}
