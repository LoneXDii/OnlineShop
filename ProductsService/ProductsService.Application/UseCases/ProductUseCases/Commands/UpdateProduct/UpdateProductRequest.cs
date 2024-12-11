using MediatR;
using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProduct;

public sealed class UpdateProductRequest() : IRequest 
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double? Price { get; set; }
    public int? Quantity { get; set; }
    public IFormFile? Image { get; set; }
}
