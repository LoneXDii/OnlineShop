using MediatR;
using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.AddCategory;

public sealed record AddCategoryRequest(string Name, IFormFile? Image) : IRequest { }
