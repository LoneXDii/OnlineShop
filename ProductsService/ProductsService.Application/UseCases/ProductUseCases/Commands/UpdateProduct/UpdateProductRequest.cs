using MediatR;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProduct;

public sealed class UpdateProductRequest() : IRequest 
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double? Price { get; set; }
    public int? Quantity { get; set; }
    public Stream? Image { get; set; }
    public string? ImageContentType { get; set; }
}
