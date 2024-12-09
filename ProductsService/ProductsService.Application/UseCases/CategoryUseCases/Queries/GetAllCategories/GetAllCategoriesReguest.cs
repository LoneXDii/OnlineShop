using MediatR;
using ProductsService.Application.DTO;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetAllCategories;

public sealed record GetAllCategoriesReguest() : IRequest<List<CategoryDTO>> { }
