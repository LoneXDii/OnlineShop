using MediatR;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteAttributeFromProduct;

public sealed record DeleteAttributeFromProductRequest(int ProductId, int AttributeId) : IRequest { }
