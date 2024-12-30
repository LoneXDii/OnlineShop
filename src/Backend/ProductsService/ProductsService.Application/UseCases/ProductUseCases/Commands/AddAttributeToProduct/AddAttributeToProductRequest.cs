using MediatR;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.AddAttributeToProduct;

public sealed record AddAttributeToProductRequest(int ProductId, int AttributeId) : IRequest { }
