using MediatR;
using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.UpdateCategory;

public sealed class UpdateCategoryRequest() : IRequest 
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public IFormFile? Image { get; set; }
}
