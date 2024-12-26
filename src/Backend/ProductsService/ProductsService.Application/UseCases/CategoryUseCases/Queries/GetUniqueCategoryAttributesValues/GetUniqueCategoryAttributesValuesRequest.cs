using MediatR;
using ProductsService.Application.DTO;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetUniqueCategoryAttributesValues;

public sealed record GetUniqueCategoryAttributesValuesRequest(int CategoryId) : IRequest<List<CategoryAttributesValuesDTO>> { }
