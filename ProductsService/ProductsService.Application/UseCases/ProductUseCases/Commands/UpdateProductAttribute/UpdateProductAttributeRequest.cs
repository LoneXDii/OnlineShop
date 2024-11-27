using MediatR;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;

public sealed record UpdateProductAttributeRequest(int ProductId, int OldAttributeId, int NewAttributeId) : IRequest { }
