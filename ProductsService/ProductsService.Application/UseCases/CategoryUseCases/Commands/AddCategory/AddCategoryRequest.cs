using MediatR;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.AddCategory;

public sealed record AddCategoryRequest(string Name) : IRequest { }
