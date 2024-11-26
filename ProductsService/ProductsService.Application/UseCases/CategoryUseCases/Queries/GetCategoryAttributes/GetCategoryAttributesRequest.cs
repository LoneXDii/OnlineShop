using MediatR;
using ProductsService.Application.DTO;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetCategoryAttributes;

public sealed record GetCategoryAttributesRequest(int categoryId) : IRequest<List<AttributeDTO>> { }