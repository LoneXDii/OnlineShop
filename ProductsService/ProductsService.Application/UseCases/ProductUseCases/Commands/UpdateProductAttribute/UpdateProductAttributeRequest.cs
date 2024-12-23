using MediatR;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;

public sealed class UpdateProductAttributeRequest() : IRequest 
{
    public int ProductId { get; set; }
    public int OldAttributeId { get; set; }
    public int NewAttributeId { get; set; }
}
