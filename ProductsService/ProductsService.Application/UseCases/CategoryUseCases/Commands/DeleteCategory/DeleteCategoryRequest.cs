using MediatR;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.DeleteCategory;

public sealed record DeleteCategoryRequest(int categoryId) : IRequest { }
