using AutoMapper;
using ProductsService.Application.DTO;
using ProductsService.Application.UseCases.ProductUseCases.Commands.AddProduct;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping;

internal class PostProductMappingProfile : Profile
{
	public PostProductMappingProfile()
	{
		CreateMap<AddProductRequest, Product>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => 0))
			.ForMember(dest => dest.CategoryProducts, opt => 
				opt.MapFrom(src => src.AttributeIds.Select(id => new CategoryProduct { CategoryId = id})));
	}
}
