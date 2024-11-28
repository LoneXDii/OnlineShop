using MediatR;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.UpdateCategory;

public sealed record UpdateCategoryRequest(int CategoryId, string Name) : IRequest { }
