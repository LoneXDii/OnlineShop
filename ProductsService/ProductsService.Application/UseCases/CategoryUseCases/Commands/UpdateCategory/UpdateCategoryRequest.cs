using MediatR;
using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.UpdateCategory;

public sealed record UpdateCategoryRequest(int CategoryId, string Name, IFormFile? Image) : IRequest { }
