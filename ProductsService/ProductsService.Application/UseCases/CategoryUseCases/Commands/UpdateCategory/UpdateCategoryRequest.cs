using MediatR;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.UpdateCategory;

public sealed class UpdateCategoryRequest() : IRequest 
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public Stream? Image { get; set; }
    public string? ImageContentType { get; set; }
}
