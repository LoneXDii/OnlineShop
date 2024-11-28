using AutoMapper;
using ProductsService.Application.UseCases.ProductUseCases.Commands.AddProduct;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.Mapping;

internal class AddProductRequestMappingProfile : Profile
{
	public AddProductRequestMappingProfile()
	{
		CreateMap<AddProductRequest, Product>()
			.ForMember(dest => dest.Categories, opt => 
				opt.MapFrom(src => src.Attributes.Select(attr => new Category { Id = attr })));
	}
}
