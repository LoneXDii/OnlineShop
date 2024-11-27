using MediatR;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;

public sealed record UpdateProductAttributeRequest(int ProductId, int OldValueId, int NewValueId) : IRequest { }
