using MediatR;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;

public sealed record UpdateProductAttributeRequest(int ProductAttributeId, string Value) : IRequest { }
